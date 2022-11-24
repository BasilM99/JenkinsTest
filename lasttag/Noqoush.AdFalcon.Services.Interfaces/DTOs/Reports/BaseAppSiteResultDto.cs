using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class BaseAppSiteResultDto: BaseReportResult
    {



        [DataMember]
        public long AdImpress { get; set; }

        [DataMember]
        public decimal Revenue { get; set; }

        [DataMember]
        public decimal NetCost { get; set; }

        [DataMember]
        public long AdRequests { get; set; }

        [DataMember]
        public long Clicks { get; set; }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get { return AdRequests == 0 ? 0 : ((decimal)AdImpress / AdRequests); } }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get { return AdImpress == 0 ? 0 : ((decimal)Clicks / AdImpress); } }

        public decimal eCPM { get { return (AdImpress == 0 ? 0 : (Revenue / AdImpress)) * 1000; } }
        public decimal eCPMNew { get { return (AdImpress == 0 ? 0 : (NetCost / AdImpress)) * 1000; } }
        [DataMember]
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



        [DataMember]
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
        [DataMember]
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

        [DataMember]
        public long WonImpressions { get; set; }
        [DataMember]
        public long RequestsByType { get; set; }


    }


    [DataContract]
    public class BaseReportResult
    {

        public void CalculateTheName(AdTypeGroup adTypeGroup)

        {
            this.Name = adTypeGroup.ToText();
        }
        public bool IsExport = false;

        private string _name = string.Empty;

        [DataMember]
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

        [DataMember]
        public string DateRange { get; set; }
        private string _subname = string.Empty;
        [DataMember]
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
        [DataMember]
        public int Id
        {
            get;
            set;
        }

    }

}
