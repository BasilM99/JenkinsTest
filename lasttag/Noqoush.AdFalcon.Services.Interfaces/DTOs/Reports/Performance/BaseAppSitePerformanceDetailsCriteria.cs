using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [DataContract]
    public class BaseAppSitePerformanceDetailsCriteria : BasePagingCriteriaDto
    {
        [DataMember]
        public IEnumerable<int> AccountIds { get; set; }

        [DataMember]
        public IEnumerable<int> AppSiteIds { get; set; }

        [DataMember]
        public IEnumerable<int> CountryIds { get; set; }

        [DataMember]
        public IEnumerable<int> PlatformIds { get; set; }

        [DataMember]
        public IEnumerable<int> DeviceIds { get; set; }

        [DataMember]
        public IEnumerable<int> OperatorIds { get; set; }

        [DataMember]
        public CampaignType CampaignType { get; set; }
        [DataMember]
        public CampaignType NotInCampaignType { get; set; }
        
    }
}
