using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using  Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;
namespace ArabyAds.AdFalcon.Web.Core.Helper
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Get Image absolute path.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static HtmlString GetImagePath(this IUrlHelper urlHelper, string imagePath) 
            {
                var path =
                    new HtmlString(
                                                                    urlHelper.AbsoluteContent(string.Format("~/Content/{0}/magenta/images/{1}",
                                                                    Thread.CurrentThread.CurrentUICulture.
                                                                        TwoLetterISOLanguageName, imagePath)));

            return path;
            }
        public static HtmlString GetOverrideImagePath(this IUrlHelper urlHelper, string imagePath)
        {
            var path =
                new HtmlString(
                     urlHelper.AbsoluteContent(string.Format("~/Content_OverRide/{0}/magenta/images/{1}",
                                                                Thread.CurrentThread.CurrentUICulture.
                                                                    TwoLetterISOLanguageName, imagePath)));

            return path;
        }
        public static HtmlString GetPublicPageUrl(this IUrlHelper urlHelper, string pageName)
        {
            var path =
                new HtmlString(
                     urlHelper.AbsoluteContent(string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName)));

            return path;
        }

        public static HtmlString GetPublicPageUrl(this IUrlHelper urlHelper, string pageName, string language)
        {
            var path =
                new HtmlString(
                     urlHelper.AbsoluteContent(string.Format("~/{0}/{1}", language, pageName)));

            return path;
        }

        public static HtmlString GetLangUrl(this IUrlHelper urlHelper)
        {
            //var path =
            //    new HtmlString(
            //        VirtualPathUtility.ToAbsolute(string.Format("~/{0}/{1}", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, pageName)));
            var path =  HttpContextHelper.Current.Request.GetDisplayUrl().ToString();
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
            return new HtmlString(path);
        }
    }
}
