using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;

namespace PdfMakerWcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPdfMaker
    {
        //http://localhost:59002/PdfMaker.svc/GetPdfUrl?url=http%3A%2F%2Fnews.ycombinator.com/
        
        /// <summary>
        /// *NOT* RESTFUL AS BEHIND THE SCENES THIS CREATES A FILE
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract,
         WebGet(UriTemplate = "GetPdfUrl?url={url}")]
        string GetPdfUrl(string url);

        //http://localhost:59002/PdfMaker.svc/GetPdf?url=http%3A%2F%2Fnews.ycombinator.com/

        /// <summary>
        /// *NOT* RESTFUL AS BEHIND THE SCENES THIS CREATES A FILE
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract,
         WebGet(UriTemplate = "GetPdf?url={url}")]
        Stream GetPdf(string url);


        //http://localhost:59002/GeneratedPdfs/GeneratedPdf_2012-12-19-02-55-38-610.PDF
    }
}
