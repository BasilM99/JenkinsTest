using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    [ProtoInclude(100,typeof(AdGroupSummaryDto))]
    public class AdGroupSummaryDtoBase
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public int CampaignId { get; set; }

       [ProtoMember(3)]
        public string Name { get; set; }

       [ProtoMember(4)]
        public string ActionType { get; set; }

       [ProtoMember(5)]
        public string Objective { get; set; }

       [ProtoMember(6)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Bid { get; set; }

       [ProtoMember(7)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal DiscountedBid { get; set; }

       [ProtoMember(8)]
        public string Status { get; set; }

       [ProtoMember(9)]
        public virtual decimal? DailyBudget { get; set; }

       [ProtoMember(10)]
        public virtual decimal? Budget { get; set; }
    }

    [ProtoContract]
    public class AdGroupSummaryDto : AdGroupSummaryDtoBase
    {
       [ProtoMember(1)]
        public IList<AdCreativeSummaryDto> AdsSummary { get; set; } = new List<AdCreativeSummaryDto>();
    }
}
