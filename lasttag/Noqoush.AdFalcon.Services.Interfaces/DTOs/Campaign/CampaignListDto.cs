using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignListResultDto
    {
        [DataMember]
        public IEnumerable<CampaignListDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
        [DataMember]

        public AdvertiserPerformanceDto Performance { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }
        [DataMember]
        public string AdvertiserName { get; set; }


        [DataMember]
        public int AdvertiserAccountId { get; set; }
        [DataMember]
        public string AdvertiserAccountName { get; set; }


        [DataMember]
        public IList<AdvertiserAccountDto> AdvertiserAccountItems { get; set; }

        [DataMember]
        public IList<CampaignDto> CampaignItems { get; set; }
        

    }
    [DataContract]
    public class CampaignListDto
    {
        public bool IsExport = false;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public decimal Budget { get; set; }
        [DataMember]
        public string BudgetText
        {
            get { return FormatHelper.FormatMoney(Budget, IsExport); }
            set { }
        }
        [DataMember]
        public CampaignPerformanceDto Performance { get; set; }
    }
}
