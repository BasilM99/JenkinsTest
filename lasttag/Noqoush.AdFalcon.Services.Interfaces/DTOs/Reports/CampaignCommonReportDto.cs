using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class CampaignCommonReportDto : BaseCampaignResultDto
    {
        [DataMember]
        public int subappId
        {
            get;
            set;
        }
        [DataMember]
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

        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public long Requests
        {
            get; set;
        }
        [DataMember]
        public long PageViews
        {
            get;set;
        }
        [DataMember]
        public string DataProvider
        {
            get; set;
        }

        public long getWins()
        {
            return WonImpressions < Impress ? Impress : WonImpressions;
        }

   
        [DataMember]
        public long WonImpressions { get; set; }
        [DataMember]
        public long RequestsByType { get; set; }


        [DataMember]
        public int Date { get; set; }
        [DataMember]
        public int ProviderId { get; set; }
        
        [DataMember]
        public int? TimeId { get; set; }
     
        [DataMember]
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
        
        [DataMember]
         public int SegmentId { get; set; }
        [DataMember]
        public long VCreativeViews { get; set; }


        [DataMember]
        public long VStart { get; set; }

        [DataMember]
        public long VFirstQuartile { get; set; }

        [DataMember]
        public long VMidPoint { get; set; }

        [DataMember]
        public long VThirdQuartile { get; set; }

        [DataMember]
        public long VComplete { get; set; }

        [DataMember]
        public long CustomEvents { get; set; }

        [DataMember]
        public long Install { get { return CustomEvents; } set { } }


      
        [DataMember]
        public string DateRangeProp { get { return DateRange; } set { } }
    }
}
