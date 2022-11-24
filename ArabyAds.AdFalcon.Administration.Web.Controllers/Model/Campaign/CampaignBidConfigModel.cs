using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.User;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Campaign
{
    public class CampaignBidConfigModel
    {
        public int CampaignId { get; set; }
        public int AdGroupId { get; set; }
        public IList<AccountViewModel> Accounts { get; set; }
        public AppSiteListResultDto AppSites { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> SubPublishers { get; set; }
        public IList<CampaignBidConfigDto> CampaignBidConfigList { get; set; }
        public string UpdatedCampaignBidConfiges { get; set; }
        public string DeletedCampaignBidConfigs { get; set; }
        public string InserteCampaignBidConfigs { get; set; }

        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
    }
}
