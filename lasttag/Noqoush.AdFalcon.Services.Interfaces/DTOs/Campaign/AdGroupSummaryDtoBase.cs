using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupSummaryDtoBase
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public string Objective { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Bid { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal DiscountedBid { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public virtual decimal? DailyBudget { get; set; }

        [DataMember]
        public virtual decimal? Budget { get; set; }
    }

    [DataContract]
    public class AdGroupSummaryDto : AdGroupSummaryDtoBase
    {
        [DataMember]
        public IList<AdCreativeSummaryDto> AdsSummary { get; set; }
    }
}
