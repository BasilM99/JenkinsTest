using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [ProtoContract]
    [ProtoInclude(100,typeof(PerformanceCriteria))]
    public class PerformanceCriteriaBase
    {
        public PerformanceCriteriaBase()
        {
            Ids = new List<int>();
        }
       [ProtoMember(1)]
        public IList<int> Ids { get; set; }

        public string GetIds(){return string.Join(",", Ids);}
       [ProtoMember(2)]
        public CampaignType CampaignType { get; set; }
       [ProtoMember(3)]
        public CampaignType OtherCampaignType { get; set; }
    }

    [ProtoContract]
    public class PerformanceCriteria : PerformanceCriteriaBase
    {
       [ProtoMember(1)]
        public DateTime? FromDate { get; set; }
       [ProtoMember(2)]
        public DateTime? ToDate { get; set; }
    }
}
