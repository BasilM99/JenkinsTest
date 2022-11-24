using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;
using System.Globalization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class ImpressionLogPerformanceDto
    {
        public bool IsExport = false;

        [DataMember]
        public long TotalCount { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int date { get; set; }
        [DataMember]
        public string AdvertiserName { get; set; }
        [DataMember]
        public string CampaignName { get; set; }

        [DataMember]
        public string dataProviderName { get; set; }

        [DataMember]
        public long Impressions { get; set; }

        [DataMember]
        public string UsedSegments { get; set; }



        [DataMember]
        public int billedsegmentId { get; set; }

        [DataMember]
        public string BilledSegment { get; set; }

        [DataMember]
        public decimal Revenue { get; set; }
        public string RevenueText
        {
            get { return FormatHelper.FormatMoney(Revenue, IsExport); }
        }
        [DataMember]
        public decimal grossrevenue { get; set; }
        public string grossrevenueText
        {
       

            get
            {

                if (grossrevenue > 0)
                    return FormatHelper.FormatMoney(grossrevenue, IsExport);
                else
                    return "NA";

            }
        }
        [DataMember]
        public decimal Discount { get; set; }
        public string DiscountText
        {
            get { 

                if (Discount > 0)
                    return FormatHelper.FormatMoney(Discount, IsExport);
                else
                    return "NA";

            }
        }
        [DataMember]
        public decimal avrcost { get; set; }
        public string avrcostText
        {
            get {
                if (avrcost > 0)
                    return FormatHelper.FormatMoney(avrcost, IsExport);
                else
                    return "NA";

            }
        }
        public DateTime DateObj
        {
            get
            {
                //   return date.ToString() != string.Empty ? DateTime.Parse(date.ToString()) : DateTime.Now;

                return date.ToString() != string.Empty ? DateTime.ParseExact(date.ToString(),
                                    "yyyyMMdd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None) : DateTime.Now;
            }
        }

    }
}
