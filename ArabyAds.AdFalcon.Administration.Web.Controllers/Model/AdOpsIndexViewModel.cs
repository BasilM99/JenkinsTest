using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model
{
    public class AdOpsIndexViewModel
    {
        public IList<CampaignSummaryDto> Campaigns { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string AccountName { get; set; }
        public string CampaignName { get; set; }
        public string CompanyName { get; set; }

    }
}