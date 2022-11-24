using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.SSP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.BusinessPartners
{
    public class BusinessPartnersListViewModelBase : ListViewModelBase
    {
        public string SaveUrl { get; set; }
        public int BusinessId { get; set; }
        public int AccountId { get; set; }
        public int DialogWidth { get; set; }
        public int DialogHeight { get; set; }
        public string DialogTitle { get; set; }
        public string BusinessName { get; set; }
        public int SiteId { get; set; }
        public string SiteIdStr { get; set; }
        public string SiteName { get; set; }
        public string GetUrl { get; set; }
        public int ZoneId { get; set; }
        public string ZoneIdStr { get; set; }
        public string ZoneName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; }
    }
    public class ListViewModel : BusinessPartnersListViewModelBase
    {
        public IEnumerable<PartnerDto> Items { get; set; }

    }

    public class SitesListViewModel : BusinessPartnersListViewModelBase
    {
        public PartnerSiteDto SaveDto { get; set; }
        public IEnumerable<PartnerSiteDto> Items { get; set; }

    }
    public class SitesZoneListViewModel : BusinessPartnersListViewModelBase
    {
        public SiteZoneDto SaveDto { get; set; }

    
        public IEnumerable<SiteZoneDto> Items { get; set; }

    }
    public class SiteZoneMappingsListViewModel : BusinessPartnersListViewModelBase
    {
        public AppSiteListResultDto AppSites { get; set; }
        //public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> SubPublishers { get; set; }
        public IList<CampaignAssignedAppsitesDto> AssignedAppsitesList { get; set; }
        public string UpdatedAssignedAppsites { get; set; }
        public string DeletedAssignedAppsites { get; set; }
        public string InsertedAssignedAppsites { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
        public string CompanyName { get; set; }

        public SiteZoneMappingDto SaveDto { get; set; }
        public IList<SelectListItem> Interstitials { get; set; }
        public IList<SelectListItem> DeviceTypes { get; set; }
        public IList<SelectListItem> AdTypes { get; set; }
        public IList<SelectListItem> NativeAdLayouts { get; set; }
        public IEnumerable<SiteZoneMappingDto> Items { get; set; }

    }

    public class FloorPriceListViewModel : BusinessPartnersListViewModelBase
    {
        public int BaseId { get; set; }
        public decimal Price { get; set; }
        public FloorPriceConfigDto SaveDto { get; set; }

        public IList<SelectListItem> FloorPriceConfigTypes { get; set; }

        public IEnumerable<FloorPriceConfigDto> Items { get; set; }

    }

    public class DealCampaignMappingListViewModel : BusinessPartnersListViewModelBase
    {

        public DealCampaignMappingDto SaveDto { get; set; }



        public IEnumerable<DealCampaignMappingDto> Items { get; set; }

    }

}
