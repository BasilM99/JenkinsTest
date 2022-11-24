using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;
using System.Globalization;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class ImpressionLogPerformanceDto
    {
        public bool IsExport = false;

       [ProtoMember(1)]
        public long TotalCount { get; set; }
       [ProtoMember(2)]
        public int Id { get; set; }
       [ProtoMember(3)]
        public int date { get; set; }
       [ProtoMember(4)]
        public string AdvertiserName { get; set; }
       [ProtoMember(5)]
        public string CampaignName { get; set; }

       [ProtoMember(6)]
        public string dataProviderName { get; set; }

       [ProtoMember(7)]
        public long Impressions { get; set; }

       [ProtoMember(8)]
        public string UsedSegments { get; set; }



       [ProtoMember(9)]
        public int billedsegmentId { get; set; }

       [ProtoMember(10)]
        public string BilledSegment { get; set; }

       [ProtoMember(11)]
        public decimal Revenue { get; set; }
        public string RevenueText
        {
            get { return FormatHelper.FormatMoney(Revenue, IsExport); }
        }
       [ProtoMember(12)]
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
       [ProtoMember(13)]
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
       [ProtoMember(14)]
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
