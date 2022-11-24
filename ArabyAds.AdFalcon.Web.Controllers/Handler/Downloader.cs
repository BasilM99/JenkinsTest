using System;
using System.IO;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;

namespace Noqoush.AdFalcon.Web.Controllers.Handler
{
    /// <summary>
    /// Summary description for Downloader
    /// </summary>
    public class Downloader : IHttpHandler
    {
        private IDocumentService _documentService = null;
        public Downloader()
        {
            _documentService = Framework.IoC.Instance.Resolve<IDocumentService>();
        }
        public void ProcessRequest(HttpContext context)
        {
            // Get the document Id from the query string 
            string docId = context.Request.QueryString["docId"];

            // Ensure that we were passed a document Id
            if (string.IsNullOrEmpty(docId))
                throw new FileNotFoundException();
            var doc = _documentService.Get(Convert.ToInt32(docId));
            // Ensure that the file exists 
            if (doc == null)
                throw new FileNotFoundException();

            // Set the content type 
            // context.Response.ContentType = "image/jpeg";
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("content-disposition", "attachment; filename=\"" + doc.UsedNameUp + doc.Extension + "\"");
            context.Response.BinaryWrite(doc.Content);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}