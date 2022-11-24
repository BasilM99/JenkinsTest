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
namespace ArabyAds.AdFalcon.Web.Controllers.Model.Advertiser
{
    public class AdvertiserAccountListViewModelBase : ListViewModelBase
    {
        public int AdvertiserId { get; set; }
        public int AdvertiserAccountId { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }


        public Select2ViewModel Countries { get; set; }
    }
    public class AdvertiserAccountListViewModel : AdvertiserAccountListViewModelBase
    {
        public IEnumerable<AdvertiserAccountListDto> Items { get; set; }
        public bool PreventEdit { get; set; }

    }

    public class MasterAppSiteListViewModel : AdvertiserAccountListViewModelBase
    {
        public IEnumerable<AdvertiserAccountMasterAppSiteDto> Items { get; set; }
     
        public bool PreventEdit { get; set; }
        public long  TotalAll { get; set; }

    }
    public class AudienceListViewModel : AdvertiserAccountListViewModelBase
    {
        public IEnumerable<AudienceSegmentDto> Items { get; set; }

        public bool PreventEdit { get; set; }

    }


    public class MasterAppSiteItemListViewModel : AdvertiserAccountListViewModelBase
    {
        public IEnumerable<AdvertiserAccountMasterAppSiteItemDto> Items { get; set; }
        public string  ListName { get; set; }
        public bool PreventEdit { get; set; }

    }

    public class PixelResultViewModel : AdvertiserAccountListViewModelBase
    {
        public IEnumerable<PixelDto> Items { get; set; }

        public bool PreventEdit { get; set; }

    }
}
