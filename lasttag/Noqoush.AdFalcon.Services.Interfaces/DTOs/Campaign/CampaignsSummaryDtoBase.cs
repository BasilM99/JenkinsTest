using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignsSummaryDtoBase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Budget { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal Spend { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:####################0.00}")]
        public decimal? DailyBudget { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string CampaignType { get; set; }

        [DataMember]
        public int CampaignTypeEnum { get; set; }
    }

    [DataContract]
    public class CampaignSummaryDto : CampaignsSummaryDtoBase
    {
        [DataMember]
        public IList<AdGroupSummaryDto> AdGroupsSummary { get; set; }
        [DataMember]
        public string AccountName { get; set; }
    }
}
