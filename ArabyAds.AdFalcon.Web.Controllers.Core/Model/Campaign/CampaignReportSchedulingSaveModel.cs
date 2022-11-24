using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.Campaign
{
    public class CampaignReportSchedulingSaveModel
    {

        public List<int> ColumnsIds { get; set; }
       
        public List<int> MeasuresIds { get; set; }
      
        public string QueryJsonData { get; set; }
       
        public string ColumnsIdsString { get; set; }

       
        public string MeasuresIdsString { get; set; }

        public bool IncludeId { get; set; }
        public int fact { get; set; }
        public string AllReportRecipient { get; set; }
        public string Name { get; set; }
        public string EmailSubject { get; set; }
        public string PreferedName { get; set; }
        public DateTime? SchedulingEndtDate { get; set; }

        public DateTime SchedulingStartDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? TimeSentAt { get; set; }
        public int Id { get; set; }

        public DateTime LastRunningDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsScheduled { get; set; }

        public string ReportJsonCriteria { get; set; }

        public DateTime? StartTime { get; set; }

        public RecurrenceType RecurrenceType { get; set; }
        public DateRecurrenceType DateRecurrenceType { get; set; }
        public ReportSectionType ReportSectionType { get; set; }
        public string EmailIntroduction { get; set; }
        
        public WeekDay WeekDay { get; set; }

        public int MonthDay { get; set; }

        //reportdto

        public int SummaryBy { get; set; }
        public string Layout { get; set; }
        public int AccountAdvertiserId { get; set; }
        public int AdvertiserId { get; set; }
        public string ItemsList { get; set; }


        public string AdvancedCriteria { get; set; }


        public string MetricCode { get; set; }

        public CampaignType CampaignType { get; set; }

        
        public bool GroupByName { get; set; }
        public bool IsAccumulated { get; set; }

        public string TabId { get; set; }


        public string DeviceCategory { get; set; }

        public string CriteriaOpt { get; set; }

        public DateTime FromDate { get; set; }


        public DateTime ToDate { get; set; }



        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public string metriceColumns { get; set; }


    }
}
