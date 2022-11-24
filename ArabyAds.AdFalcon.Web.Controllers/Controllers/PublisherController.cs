
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    public class PublisherController : ControllerBase
    {
        //TODO:use generic function to get this URL
        public string GetPublicPageUrl(string pageName)
        {
            return string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName);
        }
        public ActionResult Program()
        {
            return Redirect(GetPublicPageUrl("Publishers.html"));
        }
        public ActionResult Index()
        {
            return Redirect(GetPublicPageUrl("Publishers.html"));
        }
    }
}
