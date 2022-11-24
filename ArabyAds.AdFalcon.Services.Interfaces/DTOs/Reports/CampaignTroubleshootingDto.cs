
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class CampaignTroubleshootingDto
    {
        [ProtoMember(1)]
        public int DateId { get; set; }

        [ProtoMember(2)]
        public long Counter { get; set; }

        [ProtoMember(3)]
        public int ReasonId { get; set; }

        [ProtoMember(4)]
        public string ReasonDesc { get; set; }

        [ProtoMember(5)]
        public string CategoryDesc { get; set; }

        [ProtoMember(6)]
        public int CategoryId { get; set; }

        [ProtoMember(7)]
        public long Filled { get; set; }

        [ProtoMember(8)]
        public int CategoryOrder { get; set; }

        
    }

    [ProtoContract]
    public class CampaignTroubleshootingCategoryGroupingDto
    {
        [ProtoMember(1)]
        public int CategoryId { get; set; }

        [ProtoMember(2)]
        public string CategoryDesc { get; set; }

        [ProtoMember(3)]
        public string Total { get; set; }

        [ProtoMember(4)]
        public string Percentage { get; set; }

        [ProtoMember(5)]
        public List<CampaignTroubleshootingReasonGroupingDto> CampaignTroubleshootingByReason { get; set; }

        [ProtoMember(6)]
        public long Counter { get; set; }

        [ProtoMember(7)]
        public int CategoryOrder { get; set; }
    }

    [ProtoContract]
    public class CampaignTroubleshootingReasonGroupingDto
    {
        [ProtoMember(1)]
        public int ReasonId { get; set; }

        [ProtoMember(2)]
        public string ReasonDesc { get; set; }

        [ProtoMember(3)]
        public string Total { get; set; }
        
        [ProtoMember(4)]
        public string Percentage { get; set; }
        
        [ProtoMember(5)]
        public List<CampaignTroubleshootingDto> CampaignTroubleshooting { get; set; }
    }

    [ProtoContract]
    public class CampaignTroubleshootingResultDto
    {
        [ProtoMember(1)]
        public string TotalImpressions { get; set; }

        [ProtoMember(2)]
        public List<CampaignTroubleshootingCategoryGroupingDto> campaignTroubleshootingCategoryGroupingDto { get; set; }
    }
}
