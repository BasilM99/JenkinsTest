using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.User;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ArabyAds.AdFalcon.Web.Controllers.Model.PMPDeal
{
   

    public class InventorySourceModel
    {
        public int CampaignId { get; set; }
        public int AdGroupId { get; set; }
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
