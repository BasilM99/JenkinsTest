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
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.AccountManagement
{
   

    public class AcccountManagmentListViewModelBase : ListViewModelBase
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
    public class CostElementListViewModel : AcccountManagmentListViewModelBase
    {


        public IEnumerable<AccountCostElementDto> Items { get; set; }

    }

    public class FeeListViewModel : AcccountManagmentListViewModelBase
    {


        public IEnumerable<AccountFeeDto> Items { get; set; }

    }
}
