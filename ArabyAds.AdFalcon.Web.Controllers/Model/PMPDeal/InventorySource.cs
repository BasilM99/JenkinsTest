using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;

namespace Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal
{
   

    public class InventorySourceModel
    {
        public int CampaignId { get; set; }
        public IList<AccountViewModel> Accounts { get; set; }
        public AppSiteListResultDto AppSites { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> SubPublishers { get; set; }
        public IList<InventorySourceDto> InventorySourceList { get; set; }

        public IList<int> CheckedSSP { get; set; }
        public IList<UserDto> BusinessPartners { get; set; }

        public string UpdatedInventorySources { get; set; }
        public string DeletedInventorySources { get; set; }
        public string InsertedInventorySources { get; set; }
        public string SSPCheckedString { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string AppSiteName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Runtype { get; set; }
        
    }
}
