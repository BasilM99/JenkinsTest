using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
//using System.Web.Security;
using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
//using Combres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ArabyAds.AdFalcon.Web.Controllers.Handler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ITempDataProvider = Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider;
using Microsoft.AspNetCore.Mvc.Routing;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using Microsoft.AspNetCore.Rewrite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Web.Controllers.Core.Core;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.Web;
using ArabyAds.Framework.Web.JsonConverters;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace ArabyAds.AdFalcon.Web.Controllers.Core.Core
{
    public class CommonStartup
    {
        public  IWebHostEnvironment _env;

        public void CommonConfigure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            app.UseCors(builder => builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins( "http://*.adfalcon.com", "http://devweb.adfalcon.com:8889", "https://devweb.adfalcon.com:5559" ,"http://72.251.244.32:8889", "https://72.251.244.32:5559", "http://72.251.244.32:8899", "https://72.251.244.32:5569", "http://72.251.244.32:8888", "https://72.251.244.32:5553",  "http://adfalcon.com", "https://adfalcon.com",
                               "http://*.admin.adfalcon.com", "https://*.admin.adfalcon.com", "https://*.adfalcon.com", "http://localhost:5008", "http://localhost:5006", "https://local.devweb.adfalcon.com:8080","https://localhost:8080")
              .AllowAnyHeader()
            .AllowAnyMethod()
          
            .AllowCredentials().SetIsOriginAllowed(hostName => true)
          );
            // app.UseForwardedHeaders();

            //var cookiePolicyOptions = new CookiePolicyOptions
            //{
            //    Secure = CookieSecurePolicy.SameAsRequest,
            //    MinimumSameSitePolicy = SameSiteMode.None
            // };

            // app.UseCookiePolicy(cookiePolicyOptions);
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.UseRequestInterceptor("(req) => { req.headers['X-Requested-With'] = 'XMLHttpRequest'; return req; }");
            //});

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error/error", "?statusCode={0}");

            }
            else
            {
                app.UseDeveloperExceptionPage();


            }
            //var routeBuilder = new RouteBuilder(app);
            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                if (ctx.Request.GetTypedHeaders().CacheControl!=null  && ctx.Request.GetTypedHeaders().CacheControl.ToString().ToLower() =="max-age=0")
                {
                    ctx.Request.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(Int32.MaxValue)
                    };
                }
                await next();
            }
            );
            app.UseResponseCaching();
            
            app.UseRewriter(new RewriteOptions()
.Add(RewriteRules.RedirectRequests)
);
      
           //app.UseFileServer();

            app.UseDefaultFiles();


            app.UseStaticFiles(new StaticFileOptions()
            {
                HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
               
                OnPrepareResponse = (context) =>
                {



                    var cacheManager = Framework.IoC.Instance.Resolve<Framework.ConfigurationSetting.IConfigurationManager>();

                    var extension = System.IO.Path.GetExtension(context.File.PhysicalPath);
                    var headers = context.Context.Response.GetTypedHeaders();
                    string contentType = headers.ContentType.MediaType.Value;
                    if (contentType == "application/x-gzip")
                    {
                        if (context.File.Name.EndsWith("js.gz"))
                        {
                            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/javascript");
                           
                        }
                        else if (context.File.Name.EndsWith("css.gz"))
                        {
                            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/css");
                            
                        }
                        var cachedates = 7;
                        context.Context.Response.Headers.Add("Content-Encoding", "gzip");
                        headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(7),
                            MustRevalidate = true,

                        };
                        headers.Expires = ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(7);



                    }

                    if (!string.IsNullOrWhiteSpace(extension))
                    {
                        var cachedates = 7;
                        if (int.TryParse(cacheManager.GetConfigurationSetting(null, null, string.Format("{0}-CacheSpan", extension)), out cachedates))
                        {
                            //context.Response.Cache.SetCacheability(HttpCacheability.Public);
                            //context.Response.Cache.SetExpires(ArabyAds.Framework.Utilities.Environment.GetServerTime().AddDays(cachedates));
                            //context.Response.Cache.SetMaxAge(new TimeSpan(cachedates, 0, 0, 0));
                            //context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);


                       
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

            //app.UseHttpsRedirection();

            ArabyAds.Framework.Behaviors.BehaviorInvoker.AddIgnoredBehaviorType(typeof(ArabyAds.Framework.Attributes.TransactionalBehavior));
            // RegisterDataAnnotations();
            //  app.UseCors("AllowOrigin");
            loggerFactory.AddLog4NetProvider("log4net.config");
            app.UseRouting();
           
            app.UseAuthentication();
            app.UseMiddleware<AuthMiddleware>();
            app.UseAuthorization();

            app.UseResponseCompression();

            // app.UseSession();
            app.UseMvc(routes =>
            { RegisterRoutes(routes); });

            app.MapWhen(
 context => context.Request.Path.ToString().Contains("Downloader"),
 appBranch =>
 {
         // ... optionally add more middleware to this branch
         appBranch.UseMyHandler();


 });
        
            app.UseBundling(bundles =>
            {
                // loads bundles from a BundlerMinifier config file
                //bundles.LoadFromConfigFile("/bundleconfig.json", _env.ContentRootFileProvider);


                bundles.AddCss("/virtual-path/to/ReactKendo.css")
                   .Include("/KendoUI/adfalcon.css");

                bundles.AddCss("/virtual-path/to/ReactApp.css")
   .Include("/ClientReactJs/app.min.css");


               bundles.AddJs("/virtual-path/to/ReactVendors.js")
              .Include("/ClientReactJs/vendors/vendors.js");

                bundles.AddJs("/virtual-path/to/ReactApp.js")
                .Include("/ClientReactJs/Dashboard.js.gz");


                // defines a CSS bundle (you can use globbing patterns to include/exclude files)
                bundles.AddCss("/virtual-path/to/bundle.css")
                    .Include("/lib/bootstrap/dist/css/bootstrap.min.css")
                   // .Include("/Content/en/magenta/Style/Style2.css")
                    .Include("/Content/en/magenta/Style/telerik.common.min.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content/en/magenta/Style/bootstrap.css")
                    // .Include("/Content_OverRide/en/magenta/Style/telerik_Overide.css")
                    // .Include("/Content_OverRide/en/magenta/Style/telerik.common.min.css")
                    // .Include("/Content_OverRide/en/magenta/Style/Style2_overRide.css")
                    .Include("/Content/select2.min.css");

                bundles.AddCss("/virtual-path/to/en-magenta-siteCss.css")
                    .Include("/Content/en/magenta/Style/ui.daterangepicker.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css");


                bundles.AddCss("/virtual-path/to/ar-magenta-siteCss.css")
                    .Include("/Content/ar/magenta/Style/ui.daterangepicker.css")
                    .Include("/Content/ar/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content_OverRide/ar/magenta/Style/tabs.css")
                    .Include("/Content_OverRide/ar/magenta/Style/tabstyles.css");

                bundles.AddCss("/virtual-path/to/en-magenta-siteCss_OverRide.css")
                    .Include("/Content_OverRide/en/magenta/Style/ui.daterangepicker_OverRide.css")
                    .Include("/Content_OverRide/en/magenta/Style/jquery-ui-custom_OverRide.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content_OverRide/en/magenta/Style/tabs.css")
                    .Include("/Content_OverRide/en/magenta/Style/tabstyles.css")
                    .Include("/Content/en/magenta/Style/toastr.css");

                bundles.AddCss("/virtual-path/to/ar-magenta-siteCss_OverRide.css")
                    .Include("/Content_OverRide/ar/magenta/Style/ui.daterangepicker_OverRide.css")
                    .Include("/Content_OverRide/ar/magenta/Style/jquery-ui-custom_OverRide.css")

                    .Include("/Content/en/magenta/Style/toastr.css");

                bundles.AddCss("/virtual-path/to/backword-for-telerik.css")
                    .Include("/Content/en/magenta/Style/backword-for-telerik.css");

                bundles.AddCss("/virtual-path/to/k-grid.css")
                    .Include("/Content/en/magenta/Style/k-grid.css");



                // defines a LESS bundle (you should include entry point files only)
                bundles.AddLess("/virtual-path/to/en-magenta-siteLess.css")
                    .Include("/Content/en/magenta/Style/style.less");


                bundles.AddLess("/virtual-path/to/ar-magenta-siteLess.css")
                    .Include("/Content/ar/magenta/Style/style.less");


                bundles.AddLess("/virtual-path/to/en-magenta-siteLess_OverRide.css")
                    .Include("/Content_OverRide/en/magenta/Style/style_OverRide.less");

                bundles.AddLess("/virtual-path/to/ar-magenta-siteLess_OverRide.css")
                    .Include("/Content_OverRide/ar/magenta/Style/style_OverRide.less");


                // defines a JavaScript bundle
                //bundles.AddJs("/virtual-path/to/bundle.js")
                //    .Include("/lib/jquery/dist/jquery.min.js")
                //    .Include("/lib/bootstrap/dist/js/bootstrap.bundle.min.js")
                //    .Include("/js/select2.full.min.js");
                //.Include("/js/build.js");
                //.EnableEs6ModuleBundling(); - uncomment this line if you want the included files to be treated as ES6 modules (include only entry point file(s) in this case!)


                bundles.AddJs("/virtual-path/to/FullsiteJs.js")
                .Include("/Scripts/cdn/jquery-3.1.1.min.js")
                .Include("/Scripts/cdn/jquery-migrate-3.0.0.min.js")
                .Include("/Scripts/cdn/bootstrap.min.js")
                .Include("/Scripts/cdn/angular.min.js")
                       .Include("/Scripts/cdn/angular-sanitize.min.js")
                             .Include("/Scripts/cdn/angular-touch.js")
                .Include("/Scripts/cdn/angular-animate.js")


                 .Include("/Scripts/cdn/ui-grid.js")
                      .Include("/Scripts/cdn/select2.min.js")
                .Include("/Scripts/cdn/bootstrap-toggle.min.js");

                //cdn folder
                
                bundles.AddJs("/virtual-path/to/jquery-ui.min.js")
                     .Include("/Scripts/cdn/jquery-ui.min.js");

                //bundles.AddJs("/virtual-path/to/jquery.validate.min.js")
                //     .Include("/Scripts/cdn/jquery.validate.min.js");

                //bundles.AddJs("/virtual-path/to/jquery.validate.unobtrusive.min.js")
                //     .Include("/Scripts/cdn/jquery.validate.unobtrusive.min.js");

                bundles.AddJs("/virtual-path/to/globalize.min.js")
                     .Include("/Scripts/cdn/globalize.min.js");

                bundles.AddJs("/virtual-path/to/toastr.min.js")
                     .Include("/Scripts/cdn/toastr.min.js");


                //bundles.AddJs("/virtual-path/to/easyXDM.debug.js")
                //     .Include("/Scripts/EasyXDM/easyXDM.debug.js");

                // end cdn folder

                bundles.AddJs("/virtual-path/to/siteJs.js")

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

               // .Include("/Scripts/houseAd.targeting.js");



                bundles.AddJs("/virtual-path/to/adCreativeActionJs.js")

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


                bundles.AddJs("/virtual-path/to/targeting.js")
                .Include("/Scripts/targeting.js")
                .Include("/ScriptsNg/Contr/SSPGrid.js");

                bundles.AddJs("/virtual-path/to/ad.creative.js")
                .Include("/Scripts/ad.creative.js");


                bundles.AddJs("/virtual-path/to/Scripts/houseAdTargeting.js")
                .Include("/Scripts/houseAd.targeting.js");

            });
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    RequireHeaderSymmetry = false,
            //    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
            //});

            //string path = Path.Combine(env.WebRootPath, @"ClientReactJs\globalKeys.js");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            // Create the file, or overwrite if the file exists.
           // using (FileStream fs = File.Create(path))
           // {
              //  byte[] info = new UTF8Encoding(true).GetBytes("module.exports = {PUBLICURL : JSON.stringify('"+ configuration[WebHostDefaults.ServerUrlsKey].Split(';')[0] + "') ,BASEURL : JSON.stringify('"+ configuration[WebHostDefaults.ServerUrlsKey].Split(';')[0] + "')};");
                // Add some information to the file.
              //  fs.Write(info, 0, info.Length);
            //}

        }
        public void CommonServices(IServiceCollection services)
        {
            services.AddDataProtection()
     .PersistKeysToFileSystem(new DirectoryInfo(JsonConfigurationManager.AppSettings["keyFileStore"])).SetApplicationName("SharedWeb");


            services.AddCors();
            // AspNetCore.ResponseCaching.Extensions.ServiceCollectionExtensions.AddResponseCaching( services);
            services.AddResponseCaching();

            //services.AddDiskBackedMemoryResponseCaching((x, y) =>
            //{
            //    x.MaximumBodySize = 5 * 1024 * 1024; // Default of 64MB is probably way too big for most scenarios
            //});
            services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
            //services.AddResponseCompression();
            services.AddHttpContextAccessor();
            services.AddRouting();
            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
            
            services.AddKendo();
            // Replace Temp Data Provider
            var existing = services.FirstOrDefault(x => x.ServiceType == typeof(ITempDataProvider));
            services.Remove(existing);
            services.AddSingleton<ITempDataProvider, CacheTempDataProvider>();
            //services.AddCaching();
            // services.AddSession();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            if (_env.IsDevelopment())
            {
                services.AddBundling()
                .UseDefaults(_env) // see below
                .UseNUglify(null, new NUglify.JavaScript.CodeSettings {  MinifyCode = false }) // or .UseWebMarkupMin() - whichever minifier you prefer
                .AddLess();// to enable LESS support
                           // .EnableCacheHeader(TimeSpan.FromDays(1)); // to enable client-side caching

            }
            else
            {

                services.AddBundling()
                   .UseDefaults(_env) // see below
                   .UseNUglify(null, new NUglify.JavaScript.CodeSettings {  MinifyCode = false  }) // or .UseWebMarkupMin() - whichever minifier you prefer
                   .AddLess().EnableCacheHeader(TimeSpan.FromDays(1)).EnableChangeDetection();
            }


          //  services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            //services.AddMvc(options => options.Filters.Add(new HandleExceptionAttribute()));

            // var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            //.Build();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               
            })


                    .AddCookie(options =>
                    {
                      

                        options.Cookie.Name = "autfalcon";
                        options.AccessDeniedPath = "/User/Login/";
                        options.Cookie.Domain = JsonConfigurationManager.AppSettings["CookieDomain"]; /*"localhost";*//* Config.CookieDomain;*/
                        //  options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                        
                        options.Cookie.HttpOnly = false;
                        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                        options.LoginPath = "/User/Login/";
                        options.ReturnUrlParameter = "returnurl";
                        options.Cookie.Path = "/";
                       options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.SlidingExpiration = true;
                      
                       // options.IdleTimeout = new TimeSpan(7 * 24, 0, 0);
                        options.ExpireTimeSpan = new TimeSpan(7 * 24, 0, 0);
                        options.Cookie.MaxAge = new TimeSpan(7 * 24, 0, 0);
                        //options.Cookie.ExpireTimeSpan = new TimeSpan(7 * 24, 0, 0);
                        // options.EventsType = typeof(CustomCookieAuthenticationEvents);

                        //options.Events.OnRedirectToAccessDenied = context => FormsAuthHelper.RedirectToAccessDenied(context, section.GetValue<string>("BaseAuthUrl"));
                        //options.Events.OnRedirectToLogin = context => FormsAuthHelper.RedirectToLogin(context, section.GetValue<string>("BaseAuthUrl"));
                    });
            //services.AddAuthorization(c=>c.DefaultPolicy= policy );
            services.AddAuthorization();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // services.AddScoped<CustomCookieAuthenticationEvents>();

            //services.AddMvc(option => option.EnableEndpointRouting = false);
            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders = ForwardedHeaders.All;
            //    /*     ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto*/
            //    ;
            //});

        }
        protected virtual void RegisterLocalRoutes(IRouteBuilder routes)
        {
            var namespaces = new { NameSpace = "ArabyAds.AdFalcon.Web.Controllers.Controllers" };

            routes.MapRoute(name: "DealLocalizedAdver", template: "{language}/{controller}/{action}/{AdvertiseraccId}/{id?}", "", constraints: new { language = "[a-z]{2}", controller = "deals", action = "Create" });
            routes.MapRoute(name: "DealAdver", template: "{controller}/{action}/{AdvertiseraccId}/{id?}", "", constraints: new { controller = "deals", action = "Create" });



            routes.MapRoute(name: "campaignLocalizedAdver", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", "", constraints: new { language = "[a-z]{2}", controller = "campaign", action = "Create" });
            routes.MapRoute(name: "campaignAdver", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", "", constraints: new { controller = "campaign", action = "Create" });

            // routes.MapRoute("DealLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "CreateAll" }, namespaces);
            // routes.MapRoute("DealAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "CreateAll" }, namespaces);

            routes.MapRoute(name: "campaignLocalizedAdverCreate", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", "", constraints: new { language = "[a-z]{2}", controller = "campaign", action = "CreateAll" });
            routes.MapRoute(name: "campaignAdverCreate", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", "", constraints: new { controller = "campaign", action = "CreateAll" });
            routes.MapRoute(name: "campaignLocalizedAdverGet", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", "",new { language = "[a-z]{2}", controller = "campaign", action = "Get" }, namespaces);
            routes.MapRoute(name: "campaignAdverGet", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}","", new { controller = "campaign", action = "Get" }, namespaces);


            //routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "Create" }, namespaces);
            //routes.MapRoute("campaign", "{controller}/{action}/{id}/{objectid}", new { objectid = UrlParameter.Optional }, new { controller = "campaign", action = "Create" }, namespaces);
            routes.MapRoute(name: "campaignLocalized", template: "{language}/{controller}/{action}/{id}/{adGroupId}/{adId?}", "", constraints: new { language = "[a-z]{2}", controller = "campaign" });
            routes.MapRoute(name: "campaign", template: "{controller}/{action}/{id}/{adGroupId}/{adId?}", "", constraints: new { controller = "campaign" });

            routes.MapRoute(name: "houseAdLocalized", template: "{language}/{controller}/{action}/{id}/{adGroupId}/{adId?}", "", constraints: new { language = "[a-z]{2}", controller = "houseAd" });
            routes.MapRoute(name: "houseAd", template: "{controller}/{action}/{id}/{adGroupId}/{adId?}", "", constraints: new { controller = "houseAd" });


            routes.MapRoute("partnerLocalized", "{language}/{controller}/{action}/{Id}/{SiteId}/{ZoneId}", "", new { language = "[a-z]{2}", controller = "partner" });
            routes.MapRoute("partner", "{controller}/{action}/{Id}/{SiteId}/{ZoneId}","", new { controller = "partner" });


            routes.MapRoute("SiteZonesLocalized", "{language}/{controller}/{action}/{SiteId}/{Id}", new { }, new { language = "[a-z]{2}", controller = "partner" });
            routes.MapRoute("SiteZones", "{controller}/{action}/{SiteId}/{Id}", "", new { controller = "partner" });

            routes.MapRoute("Sites_DefaultLocalized", "{language}/{controller}/{action}/{Id}", new { }, new { language = "[a-z]{2}", controller = "partner" });
            routes.MapRoute("Sites", "{controller}/{action}/{Id}","", new { controller = "partner" });


            routes.MapRoute(name: "DashboardLocalizedAdver", template: "{language}/{controller}/{action}/{chartType}/{id?}", "", constraints: new { language = "[a-z]{2}", controller = "dashboard", action = "Index" });
            routes.MapRoute(name: "DashboardAdver", template: "{controller}/{action}/{chartType}/{id?}", "", constraints: new { controller = "dashboard", action = "Index" });






        }
        public virtual void RegisterRoutes(IRouteBuilder routes)
        {
            var namespaces = new { NameSpace = "ArabyAds.AdFalcon.Web.Controllers.Controllers" };

            //RouteTable.Routes.AddCombresRoute("Combres");
            // routes.("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "Error - 404", template: "NotFound", new { controller = "Error", action = "NotFound" });
            routes.MapRoute(name: "Error - ItemNotFound", template: "ItemNotFound", new { controller = "Error", action = "NotFound" });









            routes.MapRoute(name: "houseadcampaignLocalizedAdver", template: "{language}/{controller}/{action}/{id?}/{AdvertiseraccId?}", "", constraints: new { language = "[a-z]{2}", controller = "houseAd", action = "Create" });
            routes.MapRoute(name: "houseadcampaignAdver", template: "{controller}/{action}/{id?}/{AdvertiseraccId?}", "", constraints: new { controller = "houseAd", action = "Create" });


            RegisterLocalRoutes(routes);

            routes.MapRoute(name: "DefaultLocalized", template: "{language}/{controller}/{action}/{id?}", new { action = Config.DefaultAction }, new { language = "[a-z]{2}" });
            routes.MapRoute(name: "Default", template: "{controller}/{action}/{id?}", new { action = Config.DefaultAction });
            routes.MapRoute(name: "DefaultRoot", template: "", new { controller = "Default", action = Config.DefaultAction });



            routes.MapRoute(
            name: "defaultWithoutActionlocalized",
             template: "{language}/{controller}/{id?}",
             new { action = "Index" }, constraints: new { language = @"[a-z]{2}|[a-z]{2}-[a-zA-Z]{2}", id = @"\d+" }

          );


            routes.MapRoute(
             "defaultWithoutAction",
              "{controller}/{id?}",
             "", constraints: new { id = @"\d+" }

          );





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
