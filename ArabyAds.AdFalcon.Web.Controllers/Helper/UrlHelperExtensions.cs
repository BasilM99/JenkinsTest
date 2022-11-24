using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Noqoush.AdFalcon.Web.Controllers.Utilities;

namespace Noqoush.AdFalcon.Web.Helper
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Get Image absolute path.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static MvcHtmlString GetImagePath(this UrlHelper urlHelper, string imagePath)
        {
            var path =
                new MvcHtmlString(
                    VirtualPathUtility.ToAbsolute(string.Format("~/Content/{0}/magenta/images/{1}",
                                                                Thread.CurrentThread.CurrentUICulture.
                                                                    TwoLetterISOLanguageName, imagePath)));

            return path;
        }
        public static MvcHtmlString GetOverrideImagePath(this UrlHelper urlHelper, string imagePath)
        {
            var path =
                new MvcHtmlString(
                    VirtualPathUtility.ToAbsolute(string.Format("~/Content_OverRide/{0}/magenta/images/{1}",
                                                                Thread.CurrentThread.CurrentUICulture.
                                                                    TwoLetterISOLanguageName, imagePath)));

            return path;
        }
        public static MvcHtmlString GetPublicPageUrl(this UrlHelper urlHelper, string pageName)
        {
            var path =
                new MvcHtmlString(
                    VirtualPathUtility.ToAbsolute(string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName)));

            return path;
        }

        public static MvcHtmlString GetPublicPageUrl(this UrlHelper urlHelper, string pageName, string language)
        {
            var path =
                new MvcHtmlString(
                    VirtualPathUtility.ToAbsolute(string.Format("~/{0}/{1}", language, pageName)));

            return path;
        }

        public static MvcHtmlString GetLangUrl(this UrlHelper urlHelper)
        {
            //var path =
            //    new MvcHtmlString(
            //        VirtualPathUtility.ToAbsolute(string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName)));
            var path = System.Web.HttpContext.Current.Request.Url.ToString();
            var flag = false;
            if (path.Contains("/en/"))
            {
                flag = true;
                path = path.Replace("/en/", "/ar/");
            }
            else
            {
                if (path.Contains("/ar/"))
                {
                    flag = true;
                    path = path.Replace("/ar/", "/en/");
                }
            }
            if (!flag)
            {
                var lang = Config.CurrentLanguage;
                var newlang = "ar";
                if (lang == "ar")
                {
                    newlang = "en";
                }
                var root = Config.RootUrl;
                // path = path.Replace(VirtualPathUtility.ToAbsolute("~") + "/", string.Format(VirtualPathUtility.ToAbsolute("~") + "/{0}/", newlang));
                path = path.Replace(root, string.Format(root + "{0}/", newlang));
            }
            return new MvcHtmlString(path);
        }
    }
}
