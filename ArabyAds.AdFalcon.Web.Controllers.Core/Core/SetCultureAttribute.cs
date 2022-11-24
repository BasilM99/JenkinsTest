using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
namespace ArabyAds.AdFalcon.Web.Controllers.Core
{
    public class SetCultureAttribute : ActionFilterAttribute, IActionFilter
    {
        #region Enums

        /// <summary>
        /// Represents the location the culture code can be found
        /// </summary>
        public enum CultureLocation
        {
            /// <summary>
            /// This option should never be used.
            /// </summary>
            None = 0,

            /// <summary>
            /// Use when the culture code is saved in a cookie.  
            /// When using be sure to specify the CookieName property.
            /// </summary>
            Cookie = 1,

            /// <summary>
            /// Use when the culture code is specified in the query string.  
            /// When using be sure to specify the QueryParamName property.
            /// </summary>
            Query = 2,

            /// <summary>
            /// Use when the culture code is saved in session state.  
            /// When using be sure to specify the SessionParamName property.
            /// </summary>
            Session = 4,

            /// <summary>
            /// Use when the culture code is specified in the URL.  
            /// This assume a format of "{language}/{country}".
            /// When using be sure to specify the CountryActionParamName and 
            /// LanguageActionParamName properties.
            /// </summary>
            URL = 16
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// The name of the cookie containing the culture code.  Specify this value when CultureStore is set to Cookie.
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// The name of the action parameter containing the country code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string CountryActionParamName { get; set; }

        /// <summary>
        /// The CultureLocation where the culture code is to be read from.  This is required to be set.
        /// </summary>
        public CultureLocation CultureStore { get; set; }

        /// <summary>
        /// The name of the action parameter containing the language code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string LanguageActionParamName { get; set; }

        /// <summary>
        /// The name of the query string parameter containing the country code.  Specify this value when CultureStore is set to Query.
        /// </summary>
        public string QueryParamName { get; set; }

        /// <summary>
        /// The name of the session parameter containing the country code.  Specify this value when CultureStore is set to Session.
        /// </summary>
        public string SessionParamName { get; set; }

        #endregion Properties

        #region IActionFilter implementation

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CultureStore == CultureLocation.None)
                return;

            string cultureCode = GetCultureCode(filterContext);

            //now that we've collected the culture code, set the culture for the thread
            if (!string.IsNullOrEmpty(cultureCode))
            {
                try
                {
                    var culture = new CultureInfo(cultureCode);
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    ArabyAds.Framework.OperationContext.Current.CultureCode = cultureCode;
                }
                catch (Exception ex)
                {
                    var reThrow = ArabyAds.Framework.ExceptionHandling.ExceptionPolicy.HandleException(ex, ArabyAds.Framework.ExceptionHandling.ExceptionHandlingPolicies.UI);
                    if (reThrow)
                    {
                        throw;
                    }
                }
            }
        }

        #endregion IActionFilter implementation

        protected string GetCultureCode(ActionExecutingContext filterContext)
        {
            //Everything but CultureLocation.URL requires a valid HttpContextHelper
            if (CultureStore != CultureLocation.URL)
            {
                if (filterContext.HttpContext == null)
                    return string.Empty;
            }

            string cultureCode = string.Empty;

            if (CultureStore == CultureLocation.Cookie)
            {
                if (filterContext.HttpContext.Request.Cookies[CookieName] != null
                    && filterContext.HttpContext.Request.Cookies[CookieName] != string.Empty)
                {
                    cultureCode = filterContext.HttpContext.Request.Cookies[CookieName];
                }

                return cultureCode;
            }

            if (CultureStore == CultureLocation.Query)
            {
                cultureCode = filterContext.HttpContext.Request.Query[QueryParamName];
                return cultureCode ?? string.Empty;
            }

            if (CultureStore == CultureLocation.Session)
            {
                if (filterContext.HttpContext.Session.GetString(SessionParamName) != null
                    && filterContext.HttpContext.Session.GetString(SessionParamName).ToString() != string.Empty)
                {
                    cultureCode = filterContext.HttpContext.Session.GetString(SessionParamName).ToString();
                }

                return cultureCode;
            }

            //if URL it is expected the URL path will contain the culture 
            if (CultureStore == CultureLocation.URL)
            {
                if (filterContext.ActionArguments[LanguageActionParamName] != null &&
                    filterContext.ActionArguments[CountryActionParamName] != null
                    && filterContext.ActionArguments[LanguageActionParamName].ToString() != string.Empty &&
                    filterContext.ActionArguments[CountryActionParamName].ToString() != string.Empty
                    )
                {
                    string language = filterContext.ActionArguments[LanguageActionParamName].ToString();
                    string country = filterContext.ActionArguments[CountryActionParamName].ToString();
                    cultureCode = language + "-" + country;
                }

                return cultureCode;
            }

            return cultureCode ?? string.Empty;
        }
    }

}

