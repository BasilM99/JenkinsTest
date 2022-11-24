using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.CostElemnts
{
    public class CostElementsListViewModel : ListViewModelBase
    {
        public int AdGroupId { get; set; }
        public int CampaignId { get; set; }
        public AdGroupCostElementResultDto Elements { get; set; }
    }
}
