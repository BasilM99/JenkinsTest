using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.ServiceModel;
using System.Text;
using ArabyAds.Framework.Utilities;
//using ArabyAds.Framework.WCF.ExceptionHandling;

namespace ArabyAds.AdFalcon.AdServer.Integration.Services.Interfaces
{

  
    public enum SummaryBy
    {
        Day = 1, Week = 2, Month = 3
    }

    [ProtoContract]
    public class EmailReportCriteriaDto
    {
       [ProtoMember(1)]
        public DateTime FromDate { get; set; }

       [ProtoMember(2)]
        public DateTime ToDate { get; set; }

       [ProtoMember(3)]
        public int SummaryBy { get; set; }

       [ProtoMember(4)]
        public string Layout { get; set; }

       [ProtoMember(5)]
        public string ItemsList { get; set; }
    }

    [ProtoContract]
    public class AppEmailReportDto
    {
        public bool IsExport = false;

        public int Date;
        public string NetCostText
        {
            get { return FormatHelper.FormatMoney(NetCost, IsExport); }
        }


       [ProtoMember(1)]
        public decimal NetCost { get; set; }
       [ProtoMember(2)]
        public string DateRange;

       [ProtoMember(3)]
        public Int64 TotalCount;

       [ProtoMember(4)]
        public string Name { get; set; }

       [ProtoMember(5)]
        public string SubName { get; set; }

       [ProtoMember(6)]
        public long AdImpress { get; set; }

       [ProtoMember(7)]
        public decimal Revenue { get; set; }

       [ProtoMember(8)]
        public long AdRequests { get; set; }

       [ProtoMember(9)]
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
        //[FaultContract(typeof(ServiceFault))]
        List<AppEmailReportDto> GetAppEmailReport(EmailReportCriteriaDto criteriaDto);
    }
}
