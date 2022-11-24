using System.Collections.Generic;
using ProtoBuf;
using System;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.Framework.Utilities;
using ArabyAds.Framework.ConfigurationSetting;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CampaignListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<CampaignListDto> Items { get; set; } = new List<CampaignListDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
       [ProtoMember(3)]

        public AdvertiserPerformanceDto Performance { get; set; }
       [ProtoMember(4)]
        public int AdvertiserId { get; set; }
       [ProtoMember(5)]
        public string AdvertiserName { get; set; }


       [ProtoMember(6)]
        public int AdvertiserAccountId { get; set; }
       [ProtoMember(7)]
        public string AdvertiserAccountName { get; set; }


       [ProtoMember(8)]
        public IList<AdvertiserAccountDto> AdvertiserAccountItems { get; set; } = new List<AdvertiserAccountDto>();

        [ProtoMember(9)]
        public IList<CampaignDto> CampaignItems { get; set; } = new List<CampaignDto>();


        [ProtoMember(10)]
        public int AccountId { get; set; }

    }
    [ProtoContract]
    public class CampaignListDto
    {
        public bool IsExport = false;

       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public string Name { get; set; }
       [ProtoMember(3)]
        public DateTime CreationDate { get; set; }
       [ProtoMember(4)]
        public string Status { get; set; }
       [ProtoMember(5)]
        public decimal Budget { get; set; }
       [ProtoMember(6)]
        public string BudgetText
        {
            get { return FormatHelper.FormatMoney(Budget, IsExport); }
            set { }
        }
       [ProtoMember(7)]
        public CampaignPerformanceDto Performance { get; set; }

        [ProtoMember(8)]
        public int StatusId { get; set; }

        [ProtoMember(9)]
        public DateTime StartDate { get; set; }


    }
}
