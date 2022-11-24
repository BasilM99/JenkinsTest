using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Performance;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    [ProtoInclude(100,typeof(BaseAppSitePerformanceDetailsDto))]
    [ProtoInclude(101,typeof(AppCommonReportDto))]

    public class BaseAppSiteResultDto: BaseReportResult
    {



       [ProtoMember(1)]
        public long AdImpress { get; set; }

       [ProtoMember(2)]
        public decimal Revenue { get; set; }

       [ProtoMember(3)]
        public decimal NetCost { get; set; }

       [ProtoMember(4)]
        public long AdRequests { get; set; }

       [ProtoMember(5)]
        public long Clicks { get; set; }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get { return AdRequests == 0 ? 0 : ((decimal)AdImpress / AdRequests); } }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get { return AdImpress == 0 ? 0 : ((decimal)Clicks / AdImpress); } }

        public decimal eCPM { get { return (AdImpress == 0 ? 0 : (Revenue / AdImpress)) * 1000; } }
        public decimal eCPMNew { get { return (AdImpress == 0 ? 0 : (NetCost / AdImpress)) * 1000; } }
       [ProtoMember(6)]
        public long TotalCount { get; set; }

        public string eCPMText
        {
            get { return FormatHelper.FormatMoney(eCPM, IsExport); }
        }
        public string RevenueText
        {
            get { return FormatHelper.FormatMoney(Revenue, IsExport); }
        }
        public string NetCostText
        {
            get { return FormatHelper.FormatMoney(NetCost, IsExport); }
        }
        public string FillRateText
        {
            get { return FormatHelper.FormatPercentage(FillRate); }
        }
        public string CtrText
        {
            get { return FormatHelper.FormatPercentage(CTR); }
        }



       [ProtoMember(7)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string DisplayRate
        {
            get
            {
                if (getWins() > 0)

                    return FormatHelper.FormatPercentage(((decimal)AdImpress / getWins()));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }
       [ProtoMember(8)]
        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public string WinRate
        {
            get
            {
                if (RequestsByType > 0)


                    return FormatHelper.FormatPercentage(((decimal)getWins() / RequestsByType));
                else
                    return FormatHelper.FormatPercentage(0);
            }
            set { }
        }


        public long getWins()
        {
            return WonImpressions < AdImpress ? AdImpress : WonImpressions;
        }

       [ProtoMember(9)]
        public long WonImpressions { get; set; }
       [ProtoMember(10)]
        public long RequestsByType { get; set; }


    }


    [ProtoContract]
    [ProtoInclude(100,typeof(BaseCampaignResultDto))]
    [ProtoInclude(101, typeof(BaseAppSiteResultDto))]
    public class BaseReportResult
    {

        public void CalculateTheName(AdTypeGroup adTypeGroup)

        {
            this.Name = adTypeGroup.ToText();
        }
        public bool IsExport = false;

        private string _name = string.Empty;

       [ProtoMember(1)]
        public string Name
        {
            get
            {

                if (!string.IsNullOrEmpty(_name))
                {
                    return _name.Trim();

                }
                return string.Empty;

            }
            set
            {

                _name = value;
            }
        }

       [ProtoMember(2)]
        public string DateRange { get; set; }
        private string _subname = string.Empty;
       [ProtoMember(3)]
        public string SubName
        {
            get
            {

                if (!string.IsNullOrEmpty(_subname))
                {
                    return _subname.Trim();

                }
                return string.Empty;

            }
            set
            {

                _subname = value;
            }
        }
       [ProtoMember(4)]
        public int Id
        {
            get;
            set;
        }

    }

}
