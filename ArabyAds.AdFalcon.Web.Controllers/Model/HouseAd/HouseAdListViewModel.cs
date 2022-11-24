using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;

namespace Noqoush.AdFalcon.Web.Controllers.Model.HouseAd
{
    public class HouseAdListViewModel : ListViewModelBase
    {
        public IEnumerable<CampaignListDto> Items { get; set; }
    }
}
