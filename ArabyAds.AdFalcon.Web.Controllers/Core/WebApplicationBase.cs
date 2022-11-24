using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

using Noqoush.AdFalcon.Web.Controllers.Controllers;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.Security;
using Noqoush.Framework.Web.ClientValidation;
using Combres;
using System.Security.Permissions;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    public class WebApplicationBase : Noqoush.Framework.Web.HttpApplicationBase
    {
        static SecurityManager securityProxy;
        protected DefaultControllerFactory controllerFactory = new NoqoushControllerFactory();
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public static void RegisterDataAnnotations()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Noqoush.Framework.DataAnnotations.RequiredAttribute),
                typeof(System.Web.Mvc.RequiredAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Noqoush.Framework.DataAnnotations.RangeAttribute),
                typeof(System.Web.Mvc.RangeAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Noqoush.Framework.DataAnnotations.RegularExpressionAttribute),
                typeof(System.Web.Mvc.RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Noqoush.Framework.DataAnnotations.EmailAttribute),
                typeof(System.Web.Mvc.RegularExpressionAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(Noqoush.Framework.DataAnnotations.StringLengthAttribute),
                typeof(System.Web.Mvc.StringLengthAttributeAdapter));
            //DataAnnotationsModelValidatorProvider.RegisterAdapter(
            //    typeof(Noqoush.Framework.DataAnnotations.DataTypeAttribute), 
            //    typeof(System.Web.Mvc.));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
               typeof(Noqoush.Framework.DataAnnotations.CompareAttribute),
               typeof(CompareAttributeAdapter));



            DataAnnotationsModelValidatorProvider.RegisterAdapter(
              typeof(Noqoush.Framework.DataAnnotations.CompareAttribute),
              typeof(CompareAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
  typeof(Noqoush.Framework.DataAnnotations.RemoteAttribute),
  typeof(RemoteAttributeAdapter));

            //DataAnnotationsModelValidatorProvider.RegisterAdapter(
            //    typeof(Noqoush.Framework.DataAnnotations.DataTypeAttribute),
            //    typeof(System.Web.Mvc.ad));
        }
        protected virtual void RegisterLocalRoutes(RouteCollection routes)
        {
            string[] namespaces = { "Noqoush.AdFalcon.Web.Controllers.Controllers" };
            //routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "Create" }, namespaces);
            //routes.MapRoute("campaign", "{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { controller = "campaign", action = "Create" }, namespaces);
            routes.MapRoute("DealLocalizedAdver", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "Create" }, namespaces);
            routes.MapRoute("DealAdver", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "Create" }, namespaces);

            routes.MapRoute("campaignLocalizedAdver", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" ,action = "Create" }, namespaces);
            routes.MapRoute("campaignAdver", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { controller = "campaign", action = "Create" }, namespaces);

           // routes.MapRoute("DealLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "CreateAll" }, namespaces);
           // routes.MapRoute("DealAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "CreateAll" }, namespaces);

            routes.MapRoute("campaignLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "CreateAll" }, namespaces);
            routes.MapRoute("campaignAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { controller = "campaign", action = "CreateAll" }, namespaces);



            routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" }, namespaces);
            routes.MapRoute("campaign", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "campaign" }, namespaces);
            
            routes.MapRoute("houseAdLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "houseAd" }, namespaces);
            routes.MapRoute("houseAd", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "houseAd" }, namespaces);

        

          
        }
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            string[] namespaces = { "Noqoush.AdFalcon.Web.Controllers.Controllers" };

            RouteTable.Routes.AddCombresRoute("Combres");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Error - 404", "NotFound", new { controller = "Error", action = "NotFound" });
            routes.MapRoute("Error - ItemNotFound", "ItemNotFound", new { controller = "Error", action = "NotFound" });

            //routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" });
            //routes.MapRoute("campaign", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "campaign" });



            //routes.MapRoute("DefaultLocalizedAdvertiser", "{language}/{controller}/{action}/{id}/{AllowGlobalization}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" }, namespaces);
            //routes.MapRoute("DefaultAdvertiser", "{controller}/{action}/{id}/{AllowGlobalization}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, namespaces);

            //routes.MapRoute("dealsLocalized", "{language}/{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "Create" }, namespaces);
            //routes.MapRoute("deals", "{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { controller = "deals", action = "Create" }, namespaces);
            routes.MapRoute("houseadcampaignLocalizedAdver", "{language}/{controller}/{action}/{id}/{AdvertiseraccId}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "houseAd", action = "Create" }, namespaces);
            routes.MapRoute("houseadcampaignAdver", "{controller}/{action}/{id}/{AdvertiseraccId}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { controller = "houseAd", action = "Create" }, namespaces);



            RegisterLocalRoutes(routes);

            routes.MapRoute(
            "defaultWithoutActionlocalized",
             "{language}/{controller}/{id}",
             new { action = "Index", id = UrlParameter.Optional },constraints: new { language = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}",id = @"\d+" }

          );
            routes.MapRoute(
                    "defaultWithoutAction",
                     "{controller}/{id}",
                     new { action = "Index", id = UrlParameter.Optional }, constraints: new { id = @"\d+" }

                  );

       
            routes.MapRoute("DefaultLocalized", "{language}/{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional });
            routes.MapRoute("DefaultRoot", "", new { controller = "Default", action = Config.DefaultAction });
            //routes.MapRoute("DefaultLocalized2", "{language}/{controller}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });
            //routes.MapRoute("Default2", "{controller}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional });

            //routes.MapRoute("DefaultCampAdvertiserIdLocalized", "/{id}", new { controller = "campaign", action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });



            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(routes);
            //routes.MapRoute("DefaultCampAdvertiserId", "{controller}/{id}", new { controller = "campaign", action = Config.DefaultAction, id = UrlParameter.Optional });

        }

        protected void Session_Start()
        {
        }


        protected void Application_Init()
        {

        }

        protected void Application_Start()
        {
            
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterSecurityProxy();

            //
            //MappingRegister.RegisterMapping();
            Noqoush.Framework.Behaviors.BehaviorInvoker.AddIgnoredBehaviorType(typeof(Noqoush.Framework.Attributes.TransactionalBehavior));
            RegisterDataAnnotations();
            //ModelBinders.Binders.Add(typeof(LookupDto), new LookupModelBinder());

        }

        protected void Application_AcquireRequestState()
        {
            GetLanguage();

            var context = HttpContext.Current;

            var cacheManager = Framework.IoC.Instance.Resolve<Framework.ConfigurationSetting.IConfigurationManager>();

            var extension = System.IO.Path.GetExtension(context.Request.FilePath);
            if (!string.IsNullOrWhiteSpace(extension))
            {
                var cachedates = 7;
                if (int.TryParse(cacheManager.GetConfigurationSetting(null, null, string.Format("{0}-CacheSpan", extension)), out cachedates))
                {
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    context.Response.Cache.SetExpires(Noqoush.Framework.Utilities.Environment.GetServerTime().AddDays(cachedates));
                    context.Response.Cache.SetMaxAge(new TimeSpan(cachedates, 0, 0, 0));
                    context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                }
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
         
            var _cookie = new HttpCookie("AdFalcon_UICulture", Thread.CurrentThread.CurrentUICulture.Name) { Expires = Framework.Utilities.Environment.GetServerTime().AddYears(1), Domain = Config.CookieDomain };
            if (!HttpContext.Current.Response.IsRequestBeingRedirected && !HttpContext.Current.Response.HeadersWritten)
            HttpContext.Current.Response.SetCookie(_cookie);
        }

        private void GetLanguage()
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(System.Web.HttpContext.Current));

            if (routeData != null)
            {
                var culture = Thread.CurrentThread.CurrentCulture;
                if (routeData.Values["language"] != null &&
                    !string.IsNullOrWhiteSpace(routeData.Values["language"].ToString()))
                {
                    // set the culture from the route data (url)
                    var lang = routeData.Values["language"].ToString();
                    culture = CultureInfo.CreateSpecificCulture(getCulture(lang));
                }
                else
                {
                    // load the culture info from the cookie
                    var cookie = HttpContext.Current.Request.Cookies["AdFalcon_UICulture"];
                    var langHeader = string.Empty;
                    if (cookie != null)
                    {
                        // set the culture by the cookie content
                        langHeader = cookie.Value;
                        culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                    }
                    else
                    {
                        // set the culture by the location if not specified
                        if ((HttpContext.Current.Request.UserLanguages != null) && (HttpContext.Current.Request.UserLanguages.Any()))
                        {
                            langHeader = HttpContext.Current.Request.UserLanguages[0];
                            culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                        }
                        else
                        {
                            langHeader = "en-Us";
                            culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                        }
                    }

                    routeData.Values["language"] = langHeader;
                }

                //set Current Thread Culture
                culture.DateTimeFormat.ShortDatePattern = Config.ShortDateFormat;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Framework.OperationContext.Current.CultureCode = culture.Name;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string userSession = string.Empty;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;

                        FormsAuthenticationTicket ticket = identity.Ticket;

                        userSession = ticket.UserData;
                    }
                }
            }

            securityProxy.BuildSecurityContext(userSession);

        }

        private static void RegisterSecurityProxy()
        {
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());
        }


        private string getCulture(string lang)
        {
            lang = lang.ToLower();
            if (lang.Contains("-"))//en-US,en-UK
            {
                lang = lang.Substring(0, 2);
            }
            var returnStr = "en-US";
            switch (lang)
            {
                case "ar":
                    returnStr = "ar-JO";
                    break;
                case "en":
                    returnStr = "en-US";
                    break;
            }
            return returnStr;
        }
    }
}
