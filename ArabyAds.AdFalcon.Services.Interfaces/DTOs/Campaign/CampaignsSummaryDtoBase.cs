using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.ExceptionHandling.Exceptions;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CampaignSummaryDto))]
    public class CampaignsSummaryDtoBase
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(3)]
        public string Name { get; set; }

       [ProtoMember(4)]
        public DateTime StartDate { get; set; }

       [ProtoMember(5)]
        public DateTime? EndDate { get; set; }

       [ProtoMember(6)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Budget { get; set; }

       [ProtoMember(7)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Spend { get; set; }

       [ProtoMember(8)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal? DailyBudget { get; set; }

       [ProtoMember(9)]
        public string Status { get; set; }

       [ProtoMember(10)]
        public string CampaignType { get; set; }

       [ProtoMember(11)]
        public int CampaignTypeEnum { get; set; }
    }

    [ProtoContract]
    public class CampaignSummaryDto : CampaignsSummaryDtoBase
    {
       [ProtoMember(1)]
        public IList<AdGroupSummaryDto> AdGroupsSummary { get; set; } = new List<AdGroupSummaryDto>();
        [ProtoMember(2)]
        public string AccountName { get; set; }
    }
}
