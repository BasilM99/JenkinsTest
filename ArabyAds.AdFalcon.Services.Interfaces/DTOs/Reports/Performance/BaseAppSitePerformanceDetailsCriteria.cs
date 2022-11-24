using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance
{
    [ProtoContract]
    public class BaseAppSitePerformanceDetailsCriteria : BasePagingCriteriaDto
    {
       [ProtoMember(1)]
        public IEnumerable<int> AccountIds { get; set; }

       [ProtoMember(2)]
        public IEnumerable<int> AppSiteIds { get; set; }

       [ProtoMember(3)]
        public IEnumerable<int> CountryIds { get; set; }

       [ProtoMember(4)]
        public IEnumerable<int> PlatformIds { get; set; }

       [ProtoMember(5)]
        public IEnumerable<int> DeviceIds { get; set; }

       [ProtoMember(6)]
        public IEnumerable<int> OperatorIds { get; set; }

       [ProtoMember(7)]
        public CampaignType CampaignType { get; set; }
       [ProtoMember(8)]
        public CampaignType NotInCampaignType { get; set; }
        
    }
}
