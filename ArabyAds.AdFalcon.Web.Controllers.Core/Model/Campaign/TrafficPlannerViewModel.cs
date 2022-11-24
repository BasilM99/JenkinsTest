using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework;
using ArabyAds.Framework.DataAnnotations;

using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{

    public class TrafficPlannerViewModel
    {

        public DemographicTargetingViewModel DemographicTargetingView { get; set; }
        public Select2ViewModel Platforms { get; set; }
        public Select2ViewModel Operators { get; set; }
        public Select2ViewModel Countries { get; set; }
        public Select2ViewModel AppSites { get; set; }

        public Select2ViewModel Languages { get; set; }
        public int AdvertiserId { get; set; }
        public int AdvertiserAccountId { get; set; }

        public List<CampaignCommonReportDto> Data { get; set; }
    }


}
