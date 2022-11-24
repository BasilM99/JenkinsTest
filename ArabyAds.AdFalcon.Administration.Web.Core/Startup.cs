using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace ArabyAds.AdFalcon.Web.Core
{

    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddBundling()
                .UseDefaults(_env) // see below
                .UseNUglify() // or .UseWebMarkupMin() - whichever minifier you prefer
                .AddLess() // to enable LESS support
                .EnableCacheHeader(TimeSpan.FromDays(1)); // to enable client-side caching

            services.AddMvc(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              //  app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            var clientPath = Path.Combine(env.ContentRootPath, "en");
            var fileprovider = new PhysicalFileProvider(clientPath);
            var fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions
                             .DefaultFileNames = new[] { "index.html" };
            fileServerOptions.FileProvider = fileprovider;

            app.UseFileServer(fileServerOptions);

            app.UseMvc(RegisterRoutes);

            app.UseAuthorization();


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=AdFalconLogin}/{action=Index}/{id?}");

            //    endpoints.MapControllerRoute(
            //       name: "default2",
            //       pattern: "{controller=AdFalconLogin}/{action=Index}/{id?}");


            //});

            app.UseBundling(bundles =>
            {
                // loads bundles from a BundlerMinifier config file
                //bundles.LoadFromConfigFile("/bundleconfig.json", _env.ContentRootFileProvider);

                // defines a CSS bundle (you can use globbing patterns to include/exclude files)

               // bundles.Bundles.Add()
                bundles.AddCss("/virtual-path/to/bundle.css")
                    .Include("/lib/bootstrap/dist/css/bootstrap.min.css")
                    .Include("/Content/en/magenta/Style/Style2.css")
                    .Include("/Content/en/magenta/Style/telerik.common.min.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content/en/magenta/Style/bootstrap.css")
                    .Include("/Content_OverRide/en/magenta/Style/telerik_Overide.css")
                    .Include("/Content_OverRide/en/magenta/Style/telerik.common.min.css")
                    .Include("/Content_OverRide/en/magenta/Style/Style2_overRide.css")
                    .Include("/Content/select2.min.css");

                bundles.AddLess("/virtual-path/to/en-magenta-siteCss.css")
                    .Include("/Content/en/magenta/Style/style.less")
                    .Include("/Content/en/magenta/Style/ui.daterangepicker.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css");


                bundles.AddCss("/virtual-path/to/ar-magenta-siteCss.css")
                    .Include("/Content/ar/magenta/Style/style.less")
                    .Include("/Content/ar/magenta/Style/ui.daterangepicker.css")
                    .Include("/Content/ar/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content_OverRide/ar/magenta/Style/tabs.css")
                    .Include("/Content_OverRide/ar/magenta/Style/tabstyles.css")
                    .Include("/Content/en/magenta/Style/toastr.css");

                bundles.AddCss("/virtual-path/to/en-magenta-siteCss_OverRide.css")
                    .Include("/Content_OverRide/en/magenta/Style/style_OverRide.less")
                    .Include("/Content_OverRide/en/magenta/Style/ui.daterangepicker_OverRide.css")
                    .Include("/Content_OverRide/en/magenta/Style/jquery-ui-custom_OverRide.css")
                    .Include("/Content/en/magenta/Style/jquery-ui-custom.css")
                    .Include("/Content_OverRide/en/magenta/Style/tabs.css")
                    .Include("/Content_OverRide/en/magenta/Style/tabstyles.css")
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

            routes.MapRoute(name: "campaignLocalizedAdverGet", template: "{language}/{controller}/{action}/{AdvertiseraccId?}/{id?}", new { language = "[a-z]{2}", controller = "campaign", action = "Get" }, namespaces);
            routes.MapRoute(name: "campaignAdverGet", template: "{controller}/{action}/{AdvertiseraccId?}/{id?}", new { controller = "campaign", action = "Get" }, namespaces);

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

            routes.MapRoute(name: "Default", template: "{controller}/{action}/", new { controller = "User", action = "Login" });
            routes.MapRoute(name: "Login", template: "{controller}/", new { controller = "User", action = "Index" });
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



            //routes.MapRoute(name: "DefaultLocalized", template: "{language}/{controller}/{action}/{id?}", new { action = Config.DefaultAction }, new { language = "[a-z]{2}" });
            //routes.MapRoute(name: "Default", template: "{controller}/{action}/{id?}", new { action = Config.DefaultAction });
            //routes.MapRoute(name: "DefaultRoot", template: "", new { controller = "Default", action = Config.DefaultAction });
            //routes.MapRoute("DefaultLocalized2", "{language}/{controller}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });
            //routes.MapRoute("Default2", "{controller}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional });

            //routes.MapRoute("DefaultCampAdvertiserIdLocalized", "/{id}", new { controller = "campaign", action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}" });



            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(routes);
            //routes.MapRoute("DefaultCampAdvertiserId", "{controller}/{id}", new { controller = "campaign", action = Config.DefaultAction, id = UrlParameter.Optional });

        }

    }
}