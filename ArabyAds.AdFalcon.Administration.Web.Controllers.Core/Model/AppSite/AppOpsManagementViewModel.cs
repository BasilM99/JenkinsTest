using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.AppSite
{
    public class AppOpsManagementViewModel
    {
        public AppSiteListResultDto AppSites { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
        public string CompanyName { get; set; }

    }
}