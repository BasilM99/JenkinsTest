using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
//using System.Web.Security;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;


using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Web.ClientValidation;
//using Combres;
using Karambolo.AspNetCore.Bundling;
using System.Security.Permissions;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ITempDataProvider = Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider;
using Microsoft.AspNetCore.Mvc.Routing;

internal class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
{
    private readonly IHostingEnvironment _env;
    public ConfigureMvcOptions(IHostingEnvironment env)
    {
        _env = env;
    }

    public void Configure(MvcOptions options)
    {

        options.SslPort = 44523;

    }
}
namespace ArabyAds.AdFalcon.Web.Controllers.Core
{

    public class WebApplicationBase
    {

        private readonly IWebHostEnvironment _env;
        public WebApplicationBase(IWebHostEnvironment env)
        {
            //Configuration = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                AppContext.SetSwitch(
    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
                //services.AddResponseCompression();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddRouting();
                services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

                // Replace Temp Data Provider
                var existing = services.FirstOrDefault(x => x.ServiceType == typeof(ITempDataProvider));
                services.Remove(existing);
                services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

                services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
                services.AddBundling()
                .UseDefaults(_env) // see below
                .UseNUglify() // or .UseWebMarkupMin() - whichever minifier you prefer
                .AddLess() // to enable LESS support
                .EnableCacheHeader(TimeSpan.FromDays(1)); // to enable client-side caching


                services.AddScoped<AddCookieResultServiceFilter>();
                services.AddScoped<CurrentUserProfileFilter>();


                //services.AddMvc(options => options.Filters.Add(new HandleExceptionAttribute()));
                services
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        })
                        .AddCookie(options =>
                        {
                            options.Cookie.Name = "autfalcon";
                            options.AccessDeniedPath = "/User/Login/";
                            options.LoginPath = "/User/Login/";
                            options.ReturnUrlParameter = "returnurl";

                            options.SlidingExpiration = false;

                            //options.Events.OnRedirectToAccessDenied = context => FormsAuthHelper.RedirectToAccessDenied(context, section.GetValue<string>("BaseAuthUrl"));
                            //options.Events.OnRedirectToLogin = context => FormsAuthHelper.RedirectToLogin(context, section.GetValue<string>("BaseAuthUrl"));
                        });

                services.AddMvc(option => option.EnableEndpointRouting = false);

                services.AddControllersWithViews(options =>
                {
                    //options.Filters.Add(new AddHeaderAttribute("GlobalAddHeader",
                    //    "Result filter added to MvcOptions.Filters"));         // An instance
                    //options.Filters.Add(typeof(MySampleActionFilter));
                    //// By type

                    options.Filters.Add(new CurrentUserProfileFilter());
                    options.Filters.Add(new AddCookieResultServiceFilter());       // An instance
                });

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                AppContext.SetSwitch(
"System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                //loggerFactory.AddDebug();
                //loggerFactory.AddEventLog();

                var routeBuilder = new RouteBuilder(app);
                //routeBuilder.MapRoute("adrequest/getad", c => AdRequestController.GetAd(c));


                //ControllerBuilder.Current.SetControllerFactory(controllerFactory);
                //AreaRegistration.RegisterAllAreas();

                //RegisterGlobalFilters(GlobalFilters.Filters);
                //RegisterRoutes(RouteTable.Routes);
                //RegisterSecurityProxy();

                //
                //MappingRegister.RegisterMapping();


                ArabyAds.Framework.Behaviors.BehaviorInvoker.AddIgnoredBehaviorType(typeof(ArabyAds.Framework.Attributes.TransactionalBehavior));
                RegisterDataAnnotations();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseMvc(RegisterRoutes);
                app.UseRouter(routeBuilder.Build());



                app.UseMiddleware<Downloader>();
                app.MapWhen(
      context => context.Request.Path.ToString().StartsWith("Downloader"),
      appBranch => {
          // ... optionally add more middleware to this branch
          appBranch.UseMyHandler();


      });
                // IoC.Instance.Resolve<API.IRuntime>().Init();

                //app.UseStatusCodePagesWithReExecute("/Error");

                app.Use((context, next) =>
                {


                    RouteData routeData = context.GetRouteData();

                    if (routeData != null)
                    {

                        var userLangs = context.Request.Headers["Accept-Language"].ToString();
                        var firstLang = userLangs.Split(',').FirstOrDefault();

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
                            var cookie = context.Request.Cookies["AdFalcon_UICulture"];
                            var langHeader = string.Empty;
                            if (cookie != null)
                            {
                                // set the culture by the cookie content
                                langHeader = cookie;
                                culture = CultureInfo.CreateSpecificCulture(getCulture(langHeader));
                            }
                            else
                            {
                                // set the culture by the location if not specified
                                if ((userLangs != null) && (!string.IsNullOrEmpty(userLangs)))
                                {
                                    langHeader = firstLang;
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
                    return next();
                });


                app.UseStaticFiles(new StaticFileOptions()
                {
                    HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
                    OnPrepareResponse = (context) =>
                    {



                        var cacheManager = Framework.IoC.Instance.Resolve<Framework.ConfigurationSetting.IConfigurationManager>();

                        var extension = System.IO.Path.GetExtension(context.File.PhysicalPath);
                        if (!string.IsNullOrWhiteSpace(extension))
                        {
                            var cachedates = 7;
                            if (int.TryParse(cacheManager.GetConfigurationSetting(null, null, string.Format("{0}-CacheSpan", extension)), out cachedates))
                            {
                                //context.Response.Cache.SetCacheability(HttpCacheability.Public);
                                //context.Response.Cache.SetExpires(ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(cachedates));
                                //context.Response.Cache.SetMaxAge(new TimeSpan(cachedates, 0, 0, 0));
                                //context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);


                                var headers = context.Context.Response.GetTypedHeaders();
                                headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                                {
                                    Public = true,
                                    MaxAge = TimeSpan.FromDays(7),
                                    MustRevalidate = true,

                                };
                                headers.Expires = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(7);
                            }
                        }


                    }
                });




                //app.UseRouting();



                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();

                });

                app.UseBundling(bundles =>
                {
                    // loads bundles from a BundlerMinifier config file
                    //bundles.LoadFromConfigFile("/bundleconfig.json", _env.ContentRootFileProvider);

                    // defines a CSS bundle (you can use globbing patterns to include/exclude files)
                    //bundles.AddCss("/virtual-path/to/bundle.css")
                    //    .Include("/css/site.css")
                    //    .Include("/css/en/magenta/Style/Style2.css")
                    //    .Include("/css/en/magenta/Style/telerik.common.min.css")
                    //    .Include("/css/en/magenta/Style/jquery-ui-custom.css")
                    //    .Include("/css/en/magenta/Style/bootstrap.css")
                    //    .Include("/css/Content_OverRide/en/magenta/Style/telerik_Overide.css")
                    //    .Include("/css/Content_OverRide/en/magenta/Style/telerik.common.min.css")
                    //    .Include("/css/Content_OverRide/en/magenta/Style/Style2_overRide.css")
                    //    .Include("/css/select2.min.css");

                    bundles.AddCss("/virtual-path/to/en-magenta-siteCss.css")
                        .Include("/Content/en/magenta/Style/style.less")
                        .Include("/Content/en/magenta/Style/ui.daterangepicker.css")
                        .Include("/Content/en/magenta/Style/jquery-ui-custom.css");


                    bundles.AddCss("/virtual-path/to/ar-magenta-siteCss.css")
                        .Include("/Content/ar/magenta/Style/style.less")
                        .Include("/Content/ar/magenta/Style/ui.daterangepicker.css")
                        .Include("/Content/ar/magenta/Style/jquery-ui-custom.css")
                        .Include("/Content/Content_OverRide/ar/magenta/Style/tabs.css")
                        .Include("/Content/Content_OverRide/ar/magenta/Style/tabstyles.css")
                        .Include("/Content/en/magenta/Style/toastr.css");

                    bundles.AddCss("/virtual-path/to/en-magenta-siteCss_OverRide.css")
                        .Include("/Content/Content_OverRide/en/magenta/Style/style_OverRide.less")
                        .Include("/Content/Content_OverRide/en/magenta/Style/ui.daterangepicker_OverRide.css")
                        .Include("/Content/Content_OverRide/en/magenta/Style/jquery-ui-custom_OverRide.css")
                        .Include("/Content/en/magenta/Style/jquery-ui-custom.css")
                        .Include("/Content/Content_OverRide/en/magenta/Style/tabs.css")
                        .Include("/Content/Content_OverRide/en/magenta/Style/tabstyles.css")
                        .Include("/Content/en/magenta/Style/toastr.css");



                    // defines a LESS bundle (you should include entry point files only)
                    //bundles.AddLess("/virtual-path/to/less-bundle.css")
                    //.Include("/physical-path/to/main.less");


                    // defines a JavaScript bundle
                    //bundles.AddJs("/virtual-path/to/bundle.js")
                    //    .Include("/lib/jquery/dist/jquery.min.js")
                    //    .Include("/lib/bootstrap/dist/js/bootstrap.bundle.min.js")
                    //    .Include("/js/select2.full.min.js");
                    //.Include("/js/build.js");
                    //.EnableEs6ModuleBundling(); - uncomment this line if you want the included files to be treated as ES6 modules (include only entry point file(s) in this case!)


                    bundles.AddJs("/virtual-path/to/FullsiteJs.js")
                    .Include("/Scripts/cdn/angular.min.js")
                    .Include("/Scripts/cdn/angular-animate.js")
                    .Include("/Scripts/cdn/angular-sanitize.min.js")
                    .Include("/Scripts/cdn/angular-touch.js")
                    .Include("/Scripts/cdn/bootstrap.min.js")
                    .Include("/Scripts/cdn/bootstrap-toggle.min.js")
                    .Include("/Scripts/cdn/jquery-3.1.1.min.js")
                    .Include("/Scripts/cdn/jquery-migrate-3.0.0.min.js")
                    .Include("/Scripts/cdn/select2.min.js")
                    .Include("/Scripts/cdn/toastr.min.js")
                    .Include("/Scripts/cdn/ui-grid.js");


                    bundles.AddJs("/virtual-path/to/siteJs.js")
                    .Include("/Scripts/cdn/jquery-ui.min.js")
                    .Include("/Scripts/cdn/jquery.validate.min.js")
                    .Include("/Scripts/cdn/globalize.min.js")
                    .Include("/Scripts/jquery.sumoselect.min.js")
                    .Include("/Scripts/daterangepicker.jQuery.js")
                    .Include("/Scripts/jquery-actions.js")
                    .Include("/Scripts/jquery.ui.datepicker-Localization.js")
                    .Include("/Scripts/jquery.tree.js")
                    .Include("/Scripts/jquery.tree.checkbox.js")
                    .Include("/Scripts/jquery.json-2.2.min.js")
                    .Include("/Scripts/telerik.extensions.js")
                    .Include("/Scripts/cbpFWTabs.js")
                    .Include("/Scripts/modernizr.custom.js")
                    .Include("/Scripts/AdFalcon.js");

                    bundles.AddJs("/virtual-path/to/targetingActionJs.js")
                    .Include("/Scripts/cdn/jquery-ui.min.js")
                    .Include("/Scripts/cdn/jquery.validate.min.js")
                    .Include("/Scripts/cdn/globalize.min.js")
                    .Include("/Scripts/jquery.sumoselect.min.js")
                    .Include("/Scripts/daterangepicker.jQuery.js")
                    .Include("/Scripts/jquery-actions.js")
                    .Include("/Scripts/jquery.ui.datepicker-Localization.js")
                    .Include("/Scripts/jquery.tree.js")
                    .Include("/Scripts/jquery.tree.checkbox.js")
                    .Include("/Scripts/jquery.json-2.2.min.js")
                    .Include("/Scripts/telerik.extensions.js")
                    .Include("/Scripts/cbpFWTabs.js")
                    .Include("/Scripts/modernizr.custom.js")
                    .Include("/Scripts/AdFalcon.js")

                    .Include("/Scripts/targeting.js");


                    bundles.AddJs("/virtual-path/to/houseAdTargetingActionJs.js")
                    .Include("/Scripts/cdn/jquery-ui.min.js")
                    .Include("/Scripts/cdn/jquery.validate.min.js")
                    .Include("/Scripts/cdn/globalize.min.js")
                    .Include("/Scripts/jquery.sumoselect.min.js")
                    .Include("/Scripts/daterangepicker.jQuery.js")
                    .Include("/Scripts/jquery-actions.js")
                    .Include("/Scripts/jquery.ui.datepicker-Localization.js")
                    .Include("/Scripts/jquery.tree.js")
                    .Include("/Scripts/jquery.tree.checkbox.js")
                    .Include("/Scripts/jquery.json-2.2.min.js")
                    .Include("/Scripts/telerik.extensions.js")
                    .Include("/Scripts/cbpFWTabs.js")
                    .Include("/Scripts/modernizr.custom.js")
                    .Include("/Scripts/AdFalcon.js")

                    .Include("/Scripts/houseAd.targeting.js");



                    bundles.AddJs("/virtual-path/to/adCreativeActionJs.js")
                    .Include("/Scripts/cdn/jquery-ui.min.js")
                    .Include("/Scripts/cdn/jquery.validate.min.js")
                    .Include("/Scripts/cdn/globalize.min.js")
                    .Include("/Scripts/jquery.sumoselect.min.js")
                    .Include("/Scripts/daterangepicker.jQuery.js")
                    .Include("/Scripts/jquery-actions.js")
                    .Include("/Scripts/jquery.ui.datepicker-Localization.js")
                    .Include("/Scripts/jquery.tree.js")
                    .Include("/Scripts/jquery.tree.checkbox.js")
                    .Include("/Scripts/jquery.json-2.2.min.js")
                    .Include("/Scripts/telerik.extensions.js")
                    .Include("/Scripts/cbpFWTabs.js")
                    .Include("/Scripts/modernizr.custom.js")
                    .Include("/Scripts/AdFalcon.js")

                    .Include("/Scripts/ad.creative.js");



                    bundles.AddJs("/virtual-path/to/adCreativeSummaryJs.js")
                    .Include("/Scripts/cdn/jquery-ui.min.js")
                    .Include("/Scripts/cdn/jquery.validate.min.js")
                    .Include("/Scripts/cdn/globalize.min.js")
                    .Include("/Scripts/jquery.sumoselect.min.js")
                    .Include("/Scripts/daterangepicker.jQuery.js")
                    .Include("/Scripts/jquery-actions.js")
                    .Include("/Scripts/jquery.ui.datepicker-Localization.js")
                    .Include("/Scripts/jquery.tree.js")
                    .Include("/Scripts/jquery.tree.checkbox.js")
                    .Include("/Scripts/jquery.json-2.2.min.js")
                    .Include("/Scripts/telerik.extensions.js")
                    .Include("/Scripts/cbpFWTabs.js")
                    .Include("/Scripts/modernizr.custom.js")
                    .Include("/Scripts/AdFalcon.js");

                });

                if (!env.IsDevelopment())
                    app.UseExceptionHandler("/Error");
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        //protected DefaultControllerFactory controllerFactory = new NoqoushControllerFactory();
        // public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        // {
        //    filters.Add(new HandleErrorAttribute());
        //}
        public static void RegisterDataAnnotations()
        {

        }
        protected virtual void RegisterLocalRoutes(IRouteBuilder routes)
        {
            var namespaces = new { NameSpace = "ArabyAds.AdFalcon.Web.Controllers.Controllers" };
            //routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "Create" }, namespaces);
            //routes.MapRoute("campaign", "{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { controller = "campaign", action = "Create" }, namespaces);
            routes.MapRoute(name: "DealLocalizedAdver", template: "{language}/{controller}/{action}/{AdvertiseraccId}/{id?}", new { language = "[a-z]{2}", controller = "deals", action = "Create" }, namespaces);
            routes.MapRoute(name: "DealAdver", template: "{controller}/{action}/{AdvertiseraccId}/{id?}", new { controller = "deals", action = "Create" }, namespaces);



            routes.MapRoute(name: "campaignLocalizedAdver", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", new { language = "[a-z]{2}", controller = "campaign", action = "Create" }, namespaces);
            routes.MapRoute(name: "campaignAdver", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", new { controller = "campaign", action = "Create" }, namespaces);

            // routes.MapRoute("DealLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "CreateAll" }, namespaces);
            // routes.MapRoute("DealAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "CreateAll" }, namespaces);

            routes.MapRoute(name: "campaignLocalizedAdverCreate", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", new { language = "[a-z]{2}", controller = "campaign", action = "CreateAll" }, namespaces);
            routes.MapRoute(name: "campaignAdverCreate", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", new { controller = "campaign", action = "CreateAll" }, namespaces);
            routes.MapRoute(name: "campaignLocalizedAdverGet", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", new { language = "[a-z]{2}", controller = "campaign", action = "Get" }, namespaces);
            routes.MapRoute(name: "campaignAdverGet", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", new { controller = "campaign", action = "Get" }, namespaces);


            routes.MapRoute(name: "campaignLocalized", template: "{language}/{controller}/{action}/{id}/{adGroupId}/{adId?}", new { language = "[a-z]{2}", controller = "campaign" }, namespaces);
            routes.MapRoute(name: "campaign", template: "{controller}/{action}/{id}/{adGroupId}/{adId?}", new { controller = "campaign" }, namespaces);

            routes.MapRoute(name: "houseAdLocalized", template: "{language}/{controller}/{action}/{id}/{adGroupId}/{adId?}", new { language = "[a-z]{2}", controller = "houseAd" }, namespaces);
            routes.MapRoute(name: "houseAd", template: "{controller}/{action}/{id}/{adGroupId}/{adId?}", new { controller = "houseAd" }, namespaces);




        }
        public virtual void RegisterRoutes(IRouteBuilder routes)
        {
            var namespaces = new { NameSpace = "ArabyAds.AdFalcon.Web.Controllers.Controllers" };

            //RouteTable.Routes.AddCombresRoute("Combres");
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Error - 404", template: "NotFound", new { controller = "Error", action = "NotFound" });
            routes.MapRoute(name: "Error - ItemNotFound", template: "ItemNotFound", new { controller = "Error", action = "NotFound" });

            //routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" });
            //routes.MapRoute("campaign", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "campaign" });



            //routes.MapRoute("DefaultLocalizedAdvertiser", "{language}/{controller}/{action}/{id}/{AllowGlobalization}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" }, namespaces);
            //routes.MapRoute("DefaultAdvertiser", "{controller}/{action}/{id}/{AllowGlobalization}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, namespaces);

            //routes.MapRoute("dealsLocalized", "{language}/{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "Create" }, namespaces);
            //routes.MapRoute("deals", "{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { controller = "deals", action = "Create" }, namespaces);
            routes.MapRoute(name: "houseadcampaignLocalizedAdver", template: "{language}/{controller}/{action}/{id?}/{AdvertiseraccId?}", new { language = "[a-z]{2}", controller = "houseAd", action = "Create" }, namespaces);
            routes.MapRoute(name: "houseadcampaignAdver", template: "{controller}/{action}/{id?}/{AdvertiseraccId?}", new { controller = "houseAd", action = "Create" }, namespaces);



            RegisterLocalRoutes(routes);

            routes.MapRoute(
            name: "defaultWithoutActionlocalized",
             template: "{language}/{controller}/{id?}",
             new { action = "Index" }, constraints: new { language = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}", id = @"\d+" }

          );


            routes.MapRoute(
             "defaultWithoutAction",
              "{controller}/{id?}",
              new { language = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}", id = @"\d+" }

          );



            routes.MapRoute(name: "DefaultLocalized", template: "{language}/{controller}/{action}/{id?}", new { action = Config.DefaultAction }, new { language = "[a-z]{2}" });
            routes.MapRoute(name: "Default", template: "{controller}/{action}/{id?}", new { action = Config.DefaultAction });
            routes.MapRoute(name: "DefaultRoot", template: "", new { controller = "Default", action = Config.DefaultAction });
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


            //ModelBinders.Binders.Add(typeof(LookupDto), new LookupModelBinder());

        }

        protected void Application_AcquireRequestState()
        {
            //GetLanguage();

            //var context = HttpContextHelper.Current;

            //var cacheManager = Framework.IoC.Instance.Resolve<Framework.ConfigurationSetting.IConfigurationManager>();

            //var extension = System.IO.Path.GetExtension(context.Request.Path);
            //if (!string.IsNullOrWhiteSpace(extension))
            //{
            //    var cachedates = 7;
            //    if (int.TryParse(cacheManager.GetConfigurationSetting(null, null, string.Format("{0}-CacheSpan", extension)), out cachedates))
            //    {
            //        context.Response.Cache.SetCacheability(HttpCacheability.Public);
            //        context.Response.Cache.SetExpires(ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(cachedates));
            //        context.Response.Cache.SetMaxAge(new TimeSpan(cachedates, 0, 0, 0));
            //        context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //    }
            //}
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {


        }

        //private void GetLanguage()
        //{

        //    app.Use((context, next) =>
        //    {
        //        //get client prefered language
        //        var userLangs = context.Request.Headers["Accept-Language"].ToString();
        //        var firstLang = userLangs.Split(',').FirstOrDefault();

        //        //set allowed alanguage
        //        var lang = "en"; //default
        //        switch (firstLang)
        //        {
        //            case "hy":
        //                lang = "hy"; //allowed
        //                break;
        //            case "ru":
        //                lang = "ru"; //allowed
        //                break;
        //        }

        //        //switch culture
        //        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
        //        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //        //save for later use
        //        context.Items["ClientLang"] = lang;
        //        context.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;

        //        // Call the next delegate/middleware in the pipeline
        //        return next();
        //    });


        //}




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
