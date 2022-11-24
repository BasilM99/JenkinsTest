using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class AppSiteGeoLocationDto
    {
        public bool IsExport = false;

       [ProtoMember(1)]
        public Int64 TotalCount;

       [ProtoMember(2)]
        public string CountryName { get; set; }

       [ProtoMember(3)]
        public string AppSiteName { get; set; }

       [ProtoMember(4)]
        public long AdRequests { get; set; }
       [ProtoMember(5)]
        public decimal NetCost { get; set; }
       [ProtoMember(6)]
        public decimal Revenue { get; set; }

       [ProtoMember(7)]
        public long AdImpress { get; set; }

       [ProtoMember(8)]
        public long AdClicks { get; set; }

       [ProtoMember(9)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get; set; }

       [ProtoMember(10)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get; set; }

       [ProtoMember(11)]
        public decimal eCPM { get; set; }

       [ProtoMember(12)]
        public decimal eCPMNew { get; set; }

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

        public string NetCostText
        {
            get { return FormatHelper.FormatMoney(NetCost, IsExport); }
        }
        public string eCPMText
        {
            get { return FormatHelper.FormatMoney(eCPM, IsExport); }
        }
    }
}
