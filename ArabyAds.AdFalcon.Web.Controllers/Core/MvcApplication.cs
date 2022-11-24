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
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.AdFalcon.Web.Controllers.Controllers;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.Security;
using Noqoush.Framework.Web.ClientValidation;

namespace Noqoush.AdFalcon.Web.Controllers.Core
{
    public class MvcApplication : Noqoush.Framework.Web.HttpApplicationBase
    {
        static SecurityManager securityProxy;

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

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Error - 404", "NotFound", new { controller = "Error", action = "NotFound" });

            routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" });
            routes.MapRoute("campaign", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "campaign" });
            routes.MapRoute("DefaultLocalized", "{language}/{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional });
            routes.MapRoute("DefaultRoot", "", new { controller = "Default", action = Config.DefaultAction });

            //  routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //  routes.MapRoute("DefaultLocalized",
            //  "{language}/{controller}/{action}/{id}/{subId}",
            //  new
            //  {
            //      controller = "AppSite",
            //      action = "Index",
            //      id = UrlParameter.Optional,
            //      subId = UrlParameter.Optional
            //  },
            //  new { language = "[a-z]{2}" }
            //);
            //  routes.MapRoute(
            //      "Default", // Route name
            //      "{controller}/{action}/{id}/{subId}", // URL with parameters
            //      new
            //          {
            //              controller = "AppSite",
            //              action = "Index",
            //              id = UrlParameter.Optional,
            //              subId = UrlParameter.Optional,
            //              language = "en"
            //          } // Parameter defaults
            //  );
        }

        protected void Session_Start()
        {
            //  Session["kokoko"] = "fasdf";
        }


        protected void Application_Init()
        {

        }

        protected void Application_Start()
        {
            //Assembly[] assemblies = new Assembly[] { Assembly.Load("Noqoush.AdFalcon.Persistence"), Assembly.Load("Noqoush.Framework.DomainServices") };
            //var UoWFactory = new NHibernateUnitOfWorkFactory();
            //UoWFactory.Initialize(assemblies);
            //UnitOfWork.SetUnitOfWorkFactory(UoWFactory);
            //RegisterSecurityProxy();
            //log4net.Config.XmlConfigurator.Configure();
            //MappingRegister.RegisterMapping();
            //Framework.Behaviors.BehaviorInvoker.AddBehavior(new AuthenticationRequiredBehavior());
            //Framework.Behaviors.BehaviorInvoker.AddIgnorBehavior(typeof(Framework.Caching.CachabelBehavior));


            ControllerBuilder.Current.SetControllerFactory(new NoqoushControllerFactory());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterSecurityProxy();

            //
            MappingRegister.RegisterMapping();
            Noqoush.Framework.Behaviors.BehaviorInvoker.AddIgnorBehavior(
                typeof(Noqoush.Framework.Attributes.TransactionalBehavior));
            RegisterDataAnnotations();


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
                    context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(cachedates));
                    context.Response.Cache.SetMaxAge(new TimeSpan(cachedates, 0, 0, 0));
                    context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                }
            }
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
                        langHeader = HttpContext.Current.Request.UserLanguages[0];
                        culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                    }

                    routeData.Values["language"] = langHeader;
                }

                //set Current Thread Culture
                culture.DateTimeFormat.ShortDatePattern = Config.ShortDateFormat;
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Framework.OperationContext.Current.CultureCode = culture.Name;

                var _cookie = new HttpCookie("AdFalcon_UICulture", Thread.CurrentThread.CurrentUICulture.Name) { Expires = DateTime.Now.AddYears(1), Domain = Config.CookieDomain };
                HttpContext.Current.Response.SetCookie(_cookie);
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
