using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Web.Controllers.Model.AppSite;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.HouseAd
{
    public class HouseAdListViewModel : ListViewModelBase
    {
        public IEnumerable<CampaignListDto> Items { get; set; }
    }
}
