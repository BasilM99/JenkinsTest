using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
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
      
        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public override decimal CTR { get; set; }

    }

    [DataContract]
    public class EventItemDto 
    {
     
        [DataMember]
        
        public  string Name { get; set; }

        [DataMember]

        public string Code { get; set; }

    }
    [DataContract]
    public class AdvertiserPerformanceDto : PerformanceBaseDto
    {
        [DataMember]
        public int DimAdvID { get; set; }
        [DataMember]
        public int AdvertiserAssociationId { get; set; }
    }

    [DataContract]
    public class CampaignPerformanceDto : PerformanceBaseDto
    {
        [DataMember]
        public int DimCampaignID { get; set; }
    }

    [DataContract]
    public class AdPerformance : PerformanceBaseDto
    {
        public AdPerformance()
        {
            Bid = 0;
        }
        [DataMember]
        public int AdsID { get; set; }
        [DataMember]
        public Decimal Bid { get; set; }
    }
    [DataContract]
    public class AdGroupPerformanceDto : PerformanceBaseDto
    {
        public AdGroupPerformanceDto()
        {
            Bid = 0;
        }
        [DataMember]
        public int AdsGroupID { get; set; }
        [DataMember]
        public string Objective { get; set; }
        [DataMember]
        public decimal Bid { get; set; }

        public string BidText
        {
            get { return FormatHelper.FormatMoney(Bid, IsExport); }
        }
    }


    [DataContract]
    public class AudienceListPerformanceDto : PerformanceBaseDto
    {
        [DataMember]
        public int DimAudienceListID { get; set; }
        [DataMember]
        public int AdvertiserAssociationId { get; set; }
    }

}
