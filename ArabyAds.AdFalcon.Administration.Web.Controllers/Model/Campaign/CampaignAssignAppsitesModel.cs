using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Campaign
{
    public class CampaignAssignAppsitesModel
    {
        public int CampaignId { get; set; }
        public AppSiteListResultDto AppSites { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> SubPublishers { get; set; }
        public IList<CampaignAssignedAppsitesDto> AssignedAppsitesList { get; set; }
        public string UpdatedAssignedAppsites { get; set; }
        public string DeletedAssignedAppsites { get; set; }
        public string InsertedAssignedAppsites { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
        public string CompanyName { get; set; }

        public string NotCompatibleCampaignBidConfigs { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
   
    }

   
}
