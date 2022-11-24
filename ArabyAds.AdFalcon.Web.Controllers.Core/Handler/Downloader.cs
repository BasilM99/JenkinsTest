using System;
using System.IO;
using System.Web;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using System.Security.Claims;
using System.Security.Principal;


// ASP.NET Core middleware migrated from a handler



namespace ArabyAds.AdFalcon.Web.Controllers.Handler
{
    public class Downloader
    {
        static SecurityManager securityProxy;


        private IDocumentService _documentService = null;
        // Must have constructor with this signature, otherwise exception at run time
        public Downloader(RequestDelegate next)
        {
            // This is an HTTP Handler, so no need to store next
            _documentService = Framework.IoC.Instance.Resolve<IDocumentService>();
        }
        private static void RegisterSecurityProxy()
        {
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }
        public async Task Invoke(HttpContext context)
        {
            if (securityProxy == null)
            {
                RegisterSecurityProxy();
            }

            if (context.User != null && !(context.User is ArabyAdsPrincipal))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    if (context.User.Identity is IIdentity)
                    {
                        ClaimsIdentity identity = (ClaimsIdentity)context.User.Identity;

                        //FormsAuthenticationTicket ticket = identity.;

                        //userSession = identity.;
                        //ClaimsIdentity user = User.Identity as ClaimsIdentity

                        string userSession = identity.FindFirst("UserToken").Value;
                        securityProxy.BuildSecurityContext(userSession).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
            }
            //Get the document Id from the query string
            string docId = context.Request.Query["docId"];

            // Ensure that we were passed a document Id
            if (string.IsNullOrEmpty(docId))
                throw new FileNotFoundException();
            var doc = _documentService.Get(new ValueMessageWrapper<int> { Value = Convert.ToInt32(docId) });
            // Ensure that the file exists 
            if (doc == null)
                throw new FileNotFoundException();

            // Set the content type 
            // context.Response.ContentType = "image/jpeg";
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("content-disposition", "attachment; filename=\"" + doc.UsedNameUp + doc.Extension + "\"");
            await context.Response.Body.WriteAsync(doc.Content);
            //context.Response.Flush();
           // context.Response.End();


            //string response = GenerateResponse(context);

            //context.Response.ContentType = GetContentType();
            //await context.Response.WriteAsync(response);
        }

        // ...

        private string GenerateResponse(HttpContext context)
        {
            string title = context.Request.Query["title"];
            return string.Format("Title of the report: {0}", title);
        }

        private string GetContentType()
        {
            return "text/plain";
        }
    }

    public static class DownloaderExtensions
    {
        public static IApplicationBuilder UseMyHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Downloader>();
        }
    }
}

//namespace ArabyAds.AdFalcon.Web.Controllers.Handler
//{
//    /// <summary>
//    /// Summary description for Downloader
//    /// </summary>
//    public class Downloader : IHttpHandler
//    {
//        private IDocumentService _documentService = null;
//        public Downloader()
//        {
//            _documentService = Framework.IoC.Instance.Resolve<IDocumentService>();
//        }
//        public void ProcessRequest(HttpContextHelper context)
//        {
//            // Get the document Id from the query string 
//            string docId = context.Request.Query["docId"];

//            // Ensure that we were passed a document Id
//            if (string.IsNullOrEmpty(docId))
//                throw new FileNotFoundException();
//            var doc = _documentService.Get(Convert.ToInt32(docId));
//            // Ensure that the file exists 
//            if (doc == null)
//                throw new FileNotFoundException();

//            // Set the content type 
//            // context.Response.ContentType = "image/jpeg";
//            context.Response.ContentType = "application/octet-stream";
//            context.Response.AddHeader("content-disposition", "attachment; filename=\"" + doc.UsedNameUp + doc.Extension + "\"");
//            context.Response.BinaryWrite(doc.Content);
//            context.Response.End();
//        }

//        public bool IsReusable
//        {
//            get { return true; }
//        }
//    }
//}