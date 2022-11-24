using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class AppSiteGeoLocationDto
    {
        public bool IsExport = false;

        [DataMember]
        public Int64 TotalCount;

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string AppSiteName { get; set; }

        [DataMember]
        public long AdRequests { get; set; }
        [DataMember]
        public decimal NetCost { get; set; }
        [DataMember]
        public decimal Revenue { get; set; }

        [DataMember]
        public long AdImpress { get; set; }

        [DataMember]
        public long AdClicks { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get; set; }

        [DataMember]
        public decimal eCPM { get; set; }

        [DataMember]
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
