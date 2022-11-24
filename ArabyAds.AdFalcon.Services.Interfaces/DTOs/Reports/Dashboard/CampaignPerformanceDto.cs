using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100, typeof(AdvertiserPerformanceDto))]
    [ProtoInclude(101, typeof(CampaignPerformanceDto))]
    [ProtoInclude(102, typeof(AdPerformance))]
    [ProtoInclude(103, typeof(AdGroupPerformanceDto))]
    [ProtoInclude(104, typeof(AudienceListPerformanceDto))]
    public class PerformanceBaseDto : BaseCampaignResultDto
    {
        public PerformanceBaseDto()
        {
            Impress = 0;
            Clicks = 0;
            Spend = 0;
            BillableCost = 0;
            AvgCPC = 0;
            CTR = 0;

            UniqueUsers = 0;
            NoOfHits = 0;
        }
      
       [ProtoMember(1)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public override decimal CTR { get; set; }

    }

    [ProtoContract]
    public class EventItemDto 
    {
     
       [ProtoMember(1)]
        
        public  string Name { get; set; }

       [ProtoMember(2)]

        public string Code { get; set; }

    }
    [ProtoContract]
    public class AdvertiserPerformanceDto : PerformanceBaseDto
    {
       [ProtoMember(1)]
        public int DimAdvID { get; set; }
       [ProtoMember(2)]
        public int AdvertiserAssociationId { get; set; }
    }

    [ProtoContract]
    public class CampaignPerformanceDto : PerformanceBaseDto
    {
       [ProtoMember(1)]
        public int DimCampaignID { get; set; }
    }

    [ProtoContract]
    public class AdPerformance : PerformanceBaseDto
    {
        public AdPerformance()
        {
            Bid = 0;
        }
       [ProtoMember(1)]
        public int AdsID { get; set; }
       [ProtoMember(2)]
        public Decimal Bid { get; set; }
    }
    [ProtoContract]
    public class AdGroupPerformanceDto : PerformanceBaseDto
    {
        public AdGroupPerformanceDto()
        {
            Bid = 0;
        }
       [ProtoMember(1)]
        public int AdsGroupID { get; set; }
       [ProtoMember(2)]
        public string Objective { get; set; }
       [ProtoMember(3)]
        public decimal Bid { get; set; }

        public string BidText
        {
            get { return FormatHelper.FormatMoney(Bid, IsExport); }
        }
    }


    [ProtoContract]
    public class AudienceListPerformanceDto : PerformanceBaseDto
    {
       [ProtoMember(1)]
        public int DimAudienceListID { get; set; }
       [ProtoMember(2)]
        public int AdvertiserAssociationId { get; set; }
    }

}
