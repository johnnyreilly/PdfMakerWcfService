using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfMakerWcfService
{
    internal class PdfGenerator
    {
        /// <summary>
        /// Convert Html page at a given URL to a PDF file using open-source tool wkhtml2pdf
        ///   wkhtml2pdf can be found at: http://code.google.com/p/wkhtmltopdf/
        ///   Useful code used in the creation of this I love the good folk of StackOverflow!: http://stackoverflow.com/questions/1331926/calling-wkhtmltopdf-to-generate-pdf-from-html/1698839
        ///   An online manual can be found here: http://madalgo.au.dk/~jakobt/wkhtmltoxdoc/wkhtmltopdf-0.9.9-doc.html
        ///   
        /// Ensure that the output folder specified is writeable by the ASP.NET process of IIS running on your server
        /// 
        /// This code requires that the Windows installer is installed on the relevant server / client.  This can either be found at:
        ///   http://code.google.com/p/wkhtmltopdf/downloads/list - download wkhtmltopdf-0.9.9-installer.exe
        /// </summary>
        /// <param name="pdfOutputLocation"></param>
        /// <param name="outputFilenamePrefix"></param>
        /// <param name="urls"></param>
        /// <param name="options"></param>
        /// <param name="pdfHtmlToPdfExePath"></param>
        /// <returns>the URL of the generated PDF</returns>
        public static string HtmlToPdf(string pdfOutputLocation, string outputFilenamePrefix, string[] urls,
            string[] options = null,
            string pdfHtmlToPdfExePath = "C:\\Program Files\\wkhtmltopdf\\wkhtmltopdf.exe")/* C:\\Program Files (x86)\\wkhtmltopdf\\wkhtmltopdf.exe */
        {
            string urlsSeparatedBySpaces = string.Empty;
            try
            {
                //Determine inputs
                if ((urls == null) || (urls.Length == 0))
                    throw new Exception("No input URLs provided for HtmlToPdf");
                else
                    urlsSeparatedBySpaces = String.Join(" ", urls); //Concatenate URLs

                string outputFolder = pdfOutputLocation;
                string outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".PDF"; // assemble destination PDF file name

                var p = new System.Diagnostics.Process()
                {
                    StartInfo =
                    {
                        FileName = pdfHtmlToPdfExePath,
                        Arguments = ((options == null) ? "" : String.Join(" ", options)) + " " + urlsSeparatedBySpaces + " " + outputFilename,
                        UseShellExecute = false, // needs to be false in order to redirect output
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                        WorkingDirectory = outputFolder
                    }
                };

                p.Start();

                // read the output here...
                var output = p.StandardOutput.ReadToEnd();
                var errorOutput = p.StandardError.ReadToEnd();

                // ...then wait n milliseconds for exit (as after exit, it can't read the output)
                p.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = p.ExitCode;
                p.Close();

                // if 0 or 2, it worked so return path of pdf
                if ((returnCode == 0) || (returnCode == 2))
                    return outputFolder + outputFilename;
                else
                    throw new Exception(errorOutput);
            }
            catch (Exception exc)
            {
                throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
            }
        }
    }
}