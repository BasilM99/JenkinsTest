using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System.Web.Mvc;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Advertiser
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
