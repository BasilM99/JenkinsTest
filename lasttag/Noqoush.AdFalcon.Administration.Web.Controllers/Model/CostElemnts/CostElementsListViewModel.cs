using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Administration.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.CostElemnts
{
    public class CostElementsListViewModel : ListViewModelBase
    {
        public int AdGroupId { get; set; }
        public int CampaignId { get; set; }
        public AdGroupCostElementResultDto Elements { get; set; }
    }
}
