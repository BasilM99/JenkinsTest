using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.Resources;
using Noqoush.Framework.ConfigurationSetting;
namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    public class CampaignSchedulingDto
    {
        [DataMember]
        public ReportSchedulerDto ReportScheduler { get; set; }
        [DataMember]
        public int ID { get; private set; }
        [DataMember]
        public IList<int> Campaigns { get; set; }
        [DataMember]
        public string PreferedName { get; set; }
        [DataMember]
        public GroupByType GroupByType { get; set; }
        [DataMember]
        public ReportSummaryBy ReportSummaryBy { get; set; }
    }

    [DataContract]

    public class RecipientEmailDTO
    {
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int ID { get; set; }
   
    }
    [DataContract]

    public class ReportRecipientDTO: RecipientEmailDTO
    {
      
        [DataMember]
        public int ReportSchedulerID { get; set; }
    }
    [KnownType(typeof(RecurrenceType))]
    [KnownType(typeof(ReportSectionType))]
    [KnownType(typeof(WeekDay))]
    [DataContract]
    public class ReportSchedulerDto
    {
        [DataMember]
        public List<ReportRecipientDTO> AllReportRecipient { get; set; }
        [DataMember]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string EmailSubject { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_}{\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string PreferedName { get; set; }
        [DataMember]
        public string CompositeName { get; set; }
        [DataMember]
        public string TriggerName { get; set; }

        [DataMember]
        public int CriteriaSchedulerId { get; set; }
        //[DataMember]
        //public ReportCriteriaSchedulerDto CriteriaScheduler { get; set; }
        [DataMember]
        public string TriggerGroupName { get; set; }
        [DataMember]
        public string LanguageCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int LastDocumnetGeneratedId { get; set; }
        [DataMember]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_}{\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string Name { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime UpdateDate { get; set; }
        [DataMember]
        [Required]
        public DateTime? TimeSentAt { get; set; }
        [DataMember]
        public bool NotActiveCondition
        {
            get
            {

                return (this.EndDate != null & this.EndDate < Framework.Utilities.Environment.GetServerTime()) || (this.NextFireTime == null && this.IsScheduled) || !this.IsActive;
            }
            set { }
        }
        [DataMember]
        public string Status
        {
            get
            {

                return this.NotActiveCondition ? ResourceManager.Instance.GetResource("NotActive", "JobGrid") : ResourceManager.Instance.GetResource("Active", "JobGrid");
            }
            set { }
        }
        [DataMember]
        public string FileName
        {
            get;
            set;
        }

        [DataMember]
        public int AdvertiserAccountId
        {
            get;
            set;
        }

        [DataMember]
        public string AdvertiserName
        {
            get;
            set;
        }

        [DataMember]
        public string AdvertiserAccountName
        {
            get;
            set;
        }
        [DataMember]
        public bool IsSendNow
        {
            get;
            set;
        }

        [DataMember]
        public int UserId
        {
            get;
            set;
        }

        [DataMember]
        public bool IsPrimaryUser
        {
            get;
            set;
        }
        [DataMember]
        public string ReportTitle { get; set; }
        [DataMember]
        public bool? IsFinished { get; set; }
        [DataMember]
        public DateTime? NextFireTime { get; set; }
        [DataMember]
        public string LastRunningDateString
        {
            get;
            set;
        }

        [DataMember]
        public string EndDateString
        {
            get;
            set;
        }
        [DataMember]
        public string LastRunningIcon
        {
            get
            {
                if (NextFireTime != null & NextFireTime >= Framework.Utilities.Environment.GetServerTime())
                {
                    if (IsFinished.HasValue)
                    {
                        if (IsFinished.Value & !this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "plusmarkreporticon";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "minusreporticon";

                        }


                    }
                }

                if (NextFireTime != null & NextFireTime < Framework.Utilities.Environment.GetServerTime() )
                {
                    if (IsFinished.HasValue)
                    {
                        if (!IsFinished.Value & !this.IsSendNow)
                        {
                            return "minusreporticon";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate> NextFireTime)
                        {
                            return "plusmarkreporticon";

                        }
                    }
                    else if (!this.IsSendNow && this.LastRunningDate<= NextFireTime)
                    {
                        return "minusreporticon";

                    }
                }
          
                if (NextFireTime == null & IsFinished.HasValue)
                {
                    if (!IsFinished.Value & !this.IsSendNow)
                    {
                        return "minusreporticon";

                    }
                    if (IsFinished.Value & !this.IsSendNow)
                    {
                        return "plusmarkreporticon";

                    }
                }
            
                return "nothingreporticon";
            }
            set { }
        }
        [DataMember]
        public DateTime? LastRunningDate { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsScheduled { get; set; }
        [DataMember]
        public string ReportJsonCriteria { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public RecurrenceType RecurrenceType { get; set; }
        [DataMember]
        public ReportSectionType ReportSectionType { get; set; }
        [DataMember]
        public DateRecurrenceType DateRecurrenceType { get; set; }
        [DataMember]
        public WeekDay WeekDay { get; set; }
        [DataMember]
        public string DaysOfWeekParams { get; set; }

        [DataMember]
        public string ColorStyle
        {
            get

            {

                if (NextFireTime != null & NextFireTime >= Framework.Utilities.Environment.GetServerTime())
                {
                    if (IsFinished.HasValue)
                    {
                        if (IsFinished.Value & !this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "color:green";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "color:red";

                        }


                    }
                }

                if (NextFireTime != null & NextFireTime < Framework.Utilities.Environment.GetServerTime())
                {
                    if (IsFinished.HasValue)
                    {
                        if (!IsFinished.Value & !this.IsSendNow)
                        {
                            return "color:red";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate > NextFireTime)
                        {
                            return "color:green";

                        }
                    }
                    else if (!this.IsSendNow && this.LastRunningDate <= NextFireTime)
                    {
                        return "color:red";

                    }
                }

                if (NextFireTime == null & IsFinished.HasValue)
                {
                    if (!IsFinished.Value & !this.IsSendNow)
                    {
                        return "color:red";

                    }
                    if (IsFinished.Value & !this.IsSendNow)
                    {
                        return "color:green";

                    }
                }

                return "";
            }
            set { }
        }

        [DataMember]
        public int MonthDay { get; set; }

        [DataMember]
        public ReportCriteriaDto ReportDto { get; set; }

        [DataMember]
        public int ReportId { get; set; }

        [DataMember]
        public string URLLink { get; set; }
        [DataMember]
        public string EmailIntroduction { get; set; }

        [DataMember]
        public IList<int> MatixColumns { get; set; }
        #region FiltersMembers

        [DataMember]
        public DateTime FilterDateFrom { get; set; }
        [DataMember]
        public DateTime FilterDateTo { get; set; }
        [DataMember]
        public int FilterSize { get; set; }
        [DataMember]
        public int FilterPage { get; set; }




        #endregion

        #region DaysMembers

        [DataMember]
        public bool IsSunday { get; set; }
        [DataMember]
        public bool IsSaturday { get; set; }
        [DataMember]
        public bool IsFriday { get; set; }
        [DataMember]
        public bool IsMonday { get; set; }


        [DataMember]
        public bool IsTuesday { get; set; }
        [DataMember]
        public bool IsWednesday { get; set; }

        [DataMember]
        public bool IsThursday { get; set; }

        #endregion


        [DataMember]
        public  bool IsForQueryBuilder { get; set; }


    }

    [KnownType(typeof(RecurrenceType))]
    [KnownType(typeof(ReportSectionType))]
    [KnownType(typeof(WeekDay))]
    [DataContract]
    public class testReportSchedulerDto
    {
        [DataMember]
        public List<ReportRecipientDTO> AllReportRecipient { get; set; }
        [DataMember]
        public string PreferedName { get; set; }
        [DataMember]
        public string TriggerName { get; set; }
        [DataMember]
        public string TriggerGroupName { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int LastDocumnetGeneratedId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime? TimeSentAt { get; set; }

        [DataMember]
        public DateTime? LastRunningDate { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsScheduled { get; set; }
        [DataMember]
        public string ReportJsonCriteria { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public RecurrenceType RecurrenceType { get; set; }
        [DataMember]
        public ReportSectionType ReportSectionType { get; set; }
        [DataMember]
        public WeekDay WeekDay { get; set; }

        [DataMember]
        public int MonthDay { get; set; }

    }


    [DataContract]
    public class ResultReportSchedulerDto
    {
        [DataMember]
        public List<ReportSchedulerDto> Items { get; set; }
        [DataMember]
        public List<ReportCriteriaSchedulerDto> CriteriaItems { get; set; }
        [DataMember]
        public long TotalCount { get; set; }

    }
}
