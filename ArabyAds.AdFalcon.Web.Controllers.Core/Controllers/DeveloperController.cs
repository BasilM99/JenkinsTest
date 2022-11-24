
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = ArabyAds.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers
{
    public class DeveloperController : ControllerBase
    {
        //TODO:use generic function to get this URL
        public  string GetPublicPageUrl(string pageName)
        {
            return string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName);
        }
        public ActionResult Program()
        {
            return Redirect(GetPublicPageUrl("AppDevelopers.html"));
        }
        public ActionResult Index()
        {
            return Redirect(GetPublicPageUrl("AppDevelopers.html"));
        }
    }
}
