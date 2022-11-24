using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Account.PMP;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework;
using Noqoush.Framework.DataAnnotations;

using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Campaign
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
