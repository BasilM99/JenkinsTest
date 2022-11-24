using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class CampaignCommonReportDto : BaseCampaignResultDto
    {
       [ProtoMember(1)]
        public int subappId
        {
            get;
            set;
        }
       [ProtoMember(2)]
        public int appId
        {
            get;
            set;
        }
        public decimal eCPM { get { return (Impress == 0 ? 0 : (BillableCost / Impress)) * 1000; } }
        public string eCPMText
        {
            get { return FormatHelper.FormatMoney(eCPM, IsExport); }
        }
        public decimal eCPMNew { get { return (Impress == 0 ? 0 : (BillableCost / Impress)) * 1000; } }
        public string eCPMNewText
        {
            get { return FormatHelper.FormatMoney(eCPMNew, IsExport); }
        }

      // [ProtoMember(3)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string DisplayRate
        {
            get
            {
                if (getWins() > 0)

                return FormatHelper.FormatPercentage(((decimal)Impress / getWins()));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }
      // [ProtoMember(4)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string WinRate
        {
            get
            {
                if(RequestsByType>0)


                return FormatHelper.FormatPercentage(((decimal)getWins() / RequestsByType));
                else
                return FormatHelper.FormatPercentage(0);
            }
            set { }
        }
       [ProtoMember(3)]
        public long Requests
        {
            get; set;
        }
       [ProtoMember(4)]
        public long PageViews
        {
            get;set;
        }
       [ProtoMember(5)]
        public string DataProvider
        {
            get; set;
        }

        public long getWins()
        {
            return WonImpressions < Impress ? Impress : WonImpressions;
        }

   
       [ProtoMember(6)]
        public long WonImpressions { get; set; }
       [ProtoMember(7)]
        public long RequestsByType { get; set; }


       [ProtoMember(8)]
        public int Date { get; set; }
       [ProtoMember(9)]
        public int ProviderId { get; set; }
        
       [ProtoMember(10)]
        public int? TimeId { get; set; }
     
       [ProtoMember(11)]
        public Int64 TotalCount { get; set; }


      
        public override decimal AvgCPC
        {
            get
            {
                return Clicks == 0 ? 0 : (TotalSpend / Clicks);
            }
            set { }
        }
        public  decimal AvgCPCNew
        {
            get
            {
                return Clicks == 0 ? 0 : (BillableCost / Clicks);
            }
            set { }
        }
        public override decimal CTR
        {
            get
            {
                return Impress == 0 ? 0 : ((decimal)Clicks / Impress);
            }
            set { }
        }
        
       [ProtoMember(12)]
         public int SegmentId { get; set; }
       [ProtoMember(13)]
        public long VCreativeViews { get; set; }


       [ProtoMember(14)]
        public long VStart { get; set; }

       [ProtoMember(15)]
        public long VFirstQuartile { get; set; }

       [ProtoMember(16)]
        public long VMidPoint { get; set; }

       [ProtoMember(17)]
        public long VThirdQuartile { get; set; }

       [ProtoMember(18)]
        public long VComplete { get; set; }

       [ProtoMember(19)]
        public long CustomEvents { get; set; }

       //[ProtoMember(20)]
        public long Install { get { return CustomEvents; } set { } }


      
       //[ProtoMember(23)]
        public string DateRangeProp { get { return DateRange; } set { } }
    }
}
