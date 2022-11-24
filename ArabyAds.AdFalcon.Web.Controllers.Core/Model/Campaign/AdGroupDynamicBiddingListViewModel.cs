using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
   

    public class AdGroupDynamicBiddingListViewModel : ListViewModelBase
    {
        public int AdGroupId { get; set; }
        public int CampaignId { get; set; }
        public AdGroupDynamicBiddingConfigResultDto Elements { get; set; }
    }
}
