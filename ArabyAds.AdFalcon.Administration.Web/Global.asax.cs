using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using ArabyAds.AdFalcon.Web.Controllers.Controllers;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.AdFalcon.Web.Controllers.Utilities;
using ArabyAds.Framework;
using ArabyAds.Framework.Security;
using ArabyAds.Framework.Web.ClientValidation;
using System.IO;

namespace ArabyAds.AdFalcon.Administration.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebAdminApplicationBase : ArabyAds.AdFalcon.Web.Controllers.Core.WebApplicationBase
    {
        protected override void RegisterLocalRoutes(RouteCollection routes)
        {
            string[] namespaces = { "ArabyAds.AdFalcon.Administration.Web.Controllers.Controllers" };


            //routes.MapRoute("campaignLocalizedAdver", "{language}/{controller}/{action}/{AdvertiserId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign"}, namespaces);
            //routes.MapRoute("campaignAdver", "{controller}/{action}/{AdvertiserId}/{id}", new { id = UrlParameter.Optional }, new { controller = "campaign"}, namespaces);

            routes.MapRoute("DealLocalizedAdver", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "Create" }, namespaces);
            routes.MapRoute("DealAdver", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "Create" }, namespaces);

            routes.MapRoute("campaignLocalizedAdver", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "Create" }, namespaces);
            routes.MapRoute("campaignAdver", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { controller = "campaign", action = "Create" }, namespaces);

            /*
            routes.MapRoute("DealLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "deals", action = "CreateAll" }, namespaces);
            routes.MapRoute("DealAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional }, new { controller = "deals", action = "CreateAll" }, namespaces);

            routes.MapRoute("campaignLocalizedAdverCreate", "{language}/{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign", action = "CreateAll" }, namespaces);
            routes.MapRoute("campaignAdverCreate", "{controller}/{action}/{AdvertiseraccId}/{id}", new { id = UrlParameter.Optional, AdvertiseraccId = UrlParameter.Optional }, new { controller = "campaign", action = "CreateAll" }, namespaces);
            */


            routes.MapRoute("campaignLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" }, namespaces);
            routes.MapRoute("campaign", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "campaign" }, namespaces);
            routes.MapRoute("campaign_DefaultLocalized", "{language}/{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "campaign" }, namespaces);
            routes.MapRoute("campaign_Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { controller = "campaign" }, namespaces);


            routes.MapRoute("Party_Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { controller = "Party" }, namespaces);
            routes.MapRoute("Party_DefaultLocalized", "{language}/{controller}/{action}/{id}", new { }, new { language = "[a-z]{2}", controller = "Party" }, namespaces);


            routes.MapRoute("houseAdLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "housead" }, namespaces);
            routes.MapRoute("houseAd", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "housead" }, namespaces);
            routes.MapRoute("houseAd_DefaultLocalized", "{language}/{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "housead" }, namespaces);
            routes.MapRoute("houseAd_Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { controller = "housead" }, namespaces);


            routes.MapRoute("appSiteLocalized", "{language}/{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "appsite" }, namespaces);
            routes.MapRoute("appsite", "{controller}/{action}/{id}/{adGroupId}/{adId}", new { adId = UrlParameter.Optional }, new { controller = "appsite" }, namespaces);
            routes.MapRoute("appsite_DefaultLocalized", "{language}/{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { language = "[a-z]{2}", controller = "appsite" }, namespaces);
            routes.MapRoute("appsite_Default", "{controller}/{action}/{id}", new { action = Config.DefaultAction, id = UrlParameter.Optional }, new { controller = "appsite" }, namespaces);

            routes.MapRoute("partnerLocalized", "{language}/{controller}/{action}/{Id}/{SiteId}/{ZoneId}", new { }, new { language = "[a-z]{2}", controller = "partner" }, namespaces);
            routes.MapRoute("partner", "{controller}/{action}/{Id}/{SiteId}/{ZoneId}",  new { },new { controller = "partner" }, namespaces);


            routes.MapRoute("SiteZonesLocalized", "{language}/{controller}/{action}/{SiteId}/{Id}", new { }, new { language = "[a-z]{2}", controller = "partner" }, namespaces);
            routes.MapRoute("SiteZones", "{controller}/{action}/{SiteId}/{Id}", new { }, new { controller = "partner" }, namespaces);
      
            routes.MapRoute("Sites_DefaultLocalized", "{language}/{controller}/{action}/{Id}", new { }, new { language = "[a-z]{2}", controller = "partner" }, namespaces);
            routes.MapRoute("Sites", "{controller}/{action}/{Id}", new { }, new { controller = "partner" }, namespaces);
        
        
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //var ex = Server.GetLastError();
            //ApplicationContext.Instance.Logger.Error(ex.Message, ex);

        }

    }


}
