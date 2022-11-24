using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Noqoush.Framework.Utilities;
using Noqoush.Framework.WCF.ExceptionHandling;

namespace Noqoush.AdFalcon.AdServer.Integration.Services.Interfaces
{
    public enum SummaryBy
    {
        Day = 1, Week = 2, Month = 3
    }

    [DataContract]
    public class EmailReportCriteriaDto
    {
        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }

        [DataMember]
        public int SummaryBy { get; set; }

        [DataMember]
        public string Layout { get; set; }

        [DataMember]
        public string ItemsList { get; set; }
    }

    [DataContract]
    public class AppEmailReportDto
    {
        public bool IsExport = false;

        public int Date;
        public string NetCostText
        {
            get { return FormatHelper.FormatMoney(NetCost, IsExport); }
        }


        [DataMember]
        public decimal NetCost { get; set; }
        [DataMember]
        public string DateRange;

        [DataMember]
        public Int64 TotalCount;

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SubName { get; set; }

        [DataMember]
        public long AdImpress { get; set; }

        [DataMember]
        public decimal Revenue { get; set; }

        [DataMember]
        public long AdRequests { get; set; }

        [DataMember]
        public long AdClicks { get; set; }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal FillRate { get { return AdRequests == 0 ? 0 : ((decimal)AdImpress / AdRequests); } }

        [System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = "{0:p2}")]
        public decimal CTR { get { return AdImpress == 0 ? 0 : ((decimal)AdClicks / AdImpress); } }

        public decimal eCPM { get { return (AdImpress == 0 ? 0 : (Revenue / AdImpress)) * 1000; } }
        public decimal eCPMNew { get { return (AdImpress == 0 ? 0 : (NetCost / AdImpress)) * 1000; } }

        public string eCPMText
        {
            get { return FormatHelper.FormatMoney(eCPM, IsExport); }
        }
        public string RevenueText
        {
            get { return FormatHelper.FormatMoney(Revenue, IsExport); }
        }

        public string FillRateText
        {
            get { return FormatHelper.FormatPercentage(FillRate); }
        }
        public string CtrText
        {
            get { return FormatHelper.FormatPercentage(CTR); }
        }
    }
    [ServiceContract]
    public interface IEmailReportingService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        List<AppEmailReportDto> GetAppEmailReport(EmailReportCriteriaDto criteriaDto);
    }
}
