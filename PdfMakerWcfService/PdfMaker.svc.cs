using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;
using System.IO;

namespace PdfMakerWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class PdfMaker : IPdfMaker
    {
        private readonly Properties.Settings _settings = Properties.Settings.Default;

        /// <summary>
        /// Create a PDF based on a URL and return the URL for it
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetPdfUrl(string url)
        {
            try
            {
                //Generate PDF
                var pdfPath = GeneratePdf(url);

                //Set location header based on newly created PDF
                var location = SetLocationHeader(pdfPath);

                //Return location of newly created PDF
                return location;
            }
            catch (Exception exc)
            {
                throw new Exception("GetPdfUrl threw an exception when trying to create a PDF for this url: " + url, exc);
            }
        }

        /// <summary>
        /// Create a PDF based on a URL and return it
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream GetPdf(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    throw new ArgumentNullException();

                //Generate PDF
                var pdfPath = GeneratePdf(url);

                //Set location header based on newly created PDF
                var location = SetLocationHeader(pdfPath);

                //Return PDF mime type
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/pdf";

                //Return PDF
                return File.OpenRead(pdfPath);
            }
            catch (Exception exc)
            {
                throw new Exception("GetPdf threw an exception when trying to create a PDF for this url: " + url, exc);
            }
        }

        #region Private

        /// <summary>
        /// Create the PDF using the supplied URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GeneratePdf(string url)
        {
            //Generate PDF
            return PdfGenerator.HtmlToPdf(
                pdfOutputLocation: HostingEnvironment.MapPath(_settings.GeneratedPdfsLocation),
                outputFilenamePrefix: _settings.GeneratedPdfPrefix,
                urls: new string[] { url },
                pdfHtmlToPdfExePath: Properties.Settings.Default.PdfHtmlToPdfExePath);
        }

        /// <summary>
        /// Set location header based on the file path
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        private string SetLocationHeader(string pdfPath)
        {
            //Extract filename
            var pdfFileName = Path.GetFileName(pdfPath);

            //Determine location
            var location = new Uri(WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri,
                _settings.GeneratedPdfsLocation + pdfFileName);

            //Set location header
            WebOperationContext.Current.OutgoingResponse.Location = location.ToString();

            return WebOperationContext.Current.OutgoingResponse.Location;
        }

        #endregion
    }
}
