using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100,typeof(AppSitePerformance))]
    [ProtoInclude(101,typeof(AppSitePerformanceDto))]
    public class AppSitePerformanceBaseDto
    {
        public bool IsExport = false;

       [ProtoMember(1)]
        public Int64 TotalCount;

       [ProtoMember(2)]
        public long AdRequests { get; set; }

       [ProtoMember(3)]
        public decimal Revenue { get; set; }

       [ProtoMember(4)]
        public long AdImpress { get; set; }

       [ProtoMember(5)]
        public long AdClicks { get; set; }

       [ProtoMember(6)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get; set; }
       [ProtoMember(7)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get; set; }
       [ProtoMember(8)]
        public decimal eCPM { get; set; }

        public string CtrText
        {
            get { return FormatHelper.FormatPercentage(CTR); }
        }
        public string FillRateText
        {
            get { return FormatHelper.FormatPercentage(FillRate); }
        }
        public string RevenueText
        {
            get { return FormatHelper.FormatMoney(Revenue, IsExport); }
        }
        public string eCPMText
        {
            get { return FormatHelper.FormatMoney(eCPM, IsExport); }
        }
      
    }

    [ProtoContract]
    public class AppSitePerformance : AppSitePerformanceBaseDto
    {
       [ProtoMember(1)]
        public int AppSiteID { get; set; }
    }

    [ProtoContract]
    public class AppSitePerformanceDto : AppSitePerformanceBaseDto
    {
       [ProtoMember(1)]
        public string AppSiteName { get; set; }
    }
}
