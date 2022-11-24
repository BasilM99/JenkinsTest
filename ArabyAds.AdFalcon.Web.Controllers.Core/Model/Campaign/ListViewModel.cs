using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class CampaignListViewModelBase : ListViewModelBase
    {
        public int AdvertiserId { get; set; }
        public int AdvertiserAccountId { get; set; }
        public string AdvertiserAccountName { get; set; }

     
        public string AdvertiserName { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public bool PreventEdit { get; set; }
        
    }
    public class ListViewModel : CampaignListViewModelBase
    {
       // IDictionary<string, object> obj { get; set; }
        public IEnumerable<CampaignListDto> Items { get; set; }
        public AdvertiserPerformanceDto Performance { get; set; }

    }

    public class AdGroupListViewModel : CampaignListViewModelBase
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public IEnumerable<AdGroupListDto> Items { get; set; }
        public CampaignPerformanceDto Performance { get; set; }

    }

    public class AdListViewModel : CampaignListViewModelBase
    {
        public int AdGroupId { get; set; }
        public int CampaignId { get; set; }
        public string AdGroupName { get; set; }
        public string CampaignName { get; set; }
        public IEnumerable<AdListDto> Items { get; set; }
        public AdGroupPerformanceDto Performance { get; set; }

    }
}
