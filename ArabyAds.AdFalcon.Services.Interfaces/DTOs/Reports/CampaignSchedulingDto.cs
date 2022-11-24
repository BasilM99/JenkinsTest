using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.Framework.Resources;
using ArabyAds.Framework.ConfigurationSetting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    public class CampaignSchedulingDto
    {
       [ProtoMember(1)]
        public ReportSchedulerDto ReportScheduler { get; set; }
       [ProtoMember(2)]
        public int ID { get; private set; }
       [ProtoMember(3)]
        public IList<int> Campaigns { get; set; }
       [ProtoMember(4)]
        public string PreferedName { get; set; }
       [ProtoMember(5)]
        public GroupByType GroupByType { get; set; }
       [ProtoMember(6)]
        public ReportSummaryBy ReportSummaryBy { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100,typeof(AccountDSPsettingContactDTO))]
    [ProtoInclude(101,typeof(ReportRecipientDTO))]
    public class RecipientEmailDTO
    {
       [ProtoMember(1)]
        public bool IsDeleted { get; set; }
       [ProtoMember(2)]
        public string Email { get; set; }
       [ProtoMember(3)]
        public int ID { get; set; }
   
    }
    [ProtoContract]

    public class ReportRecipientDTO: RecipientEmailDTO
    {
      
       [ProtoMember(1)]
        public int ReportSchedulerID { get; set; }
    }
    [ProtoInclude(100,typeof(RecurrenceType))]
    [ProtoInclude(101,typeof(ReportSectionType))]
    [ProtoInclude(102,typeof(WeekDay))]
    [ProtoContract]
    public class ReportSchedulerDto
    {
        [ProtoMember(1)]
        public List<ReportRecipientDTO> AllReportRecipient { get; set; } = new List<ReportRecipientDTO>();
       [ProtoMember(2)]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string EmailSubject { get; set; }
       [ProtoMember(3)]
        public string AccountName { get; set; }
       [ProtoMember(4)]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_}{\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string PreferedName { get; set; }
       [ProtoMember(5)]
        public string CompositeName { get; set; }
       [ProtoMember(6)]
        public string TriggerName { get; set; }

       [ProtoMember(7)]
        public int CriteriaSchedulerId { get; set; }
        //[DataMember]
        //public ReportCriteriaSchedulerDto CriteriaScheduler { get; set; }
       [ProtoMember(8)]
        public string TriggerGroupName { get; set; }
       [ProtoMember(9)]
        public string LanguageCode { get; set; }
       [ProtoMember(10)]
        public string Description { get; set; }
       [ProtoMember(11)]
        public int ID { get; set; }
       [ProtoMember(12)]
        public int AccountId { get; set; }
       [ProtoMember(13)]
        public bool IsDeleted { get; set; }
       [ProtoMember(14)]
        public int LastDocumnetGeneratedId { get; set; }
       [ProtoMember(15)]
        [StringLength(255)]
        //[RegularExpression(@"^[^-\s][أ-يa-zA-Z0-9_}{\s-]+$", ResourceName = "InvalidName")]
        [Required]
        public string Name { get; set; }
       [ProtoMember(16)]
        public DateTime? EndDate { get; set; }
       [ProtoMember(17)]
        public DateTime StartDate { get; set; }
       [ProtoMember(18)]
        public DateTime CreationDate { get; set; }
       [ProtoMember(19)]
        public DateTime UpdateDate { get; set; }
       [ProtoMember(20)]
        [Required]
        public DateTime? TimeSentAt { get; set; }
       [ProtoMember(21)]
        public bool NotActiveCondition
        {
            get
            {

                return (this.EndDate != null & this.EndDate < Framework.Utilities.Environment.GetServerTime()) || (this.NextFireTime == null && this.IsScheduled) || !this.IsActive;
            }
            set { }
        }
       [ProtoMember(22)]
        public string Status
        {
            get
            {

                return this.NotActiveCondition ? ResourceManager.Instance.GetResource("NotActive", "JobGrid") : ResourceManager.Instance.GetResource("Active", "JobGrid");
            }
            set { }
        }
       [ProtoMember(23)]
        public string FileName
        {
            get;
            set;
        }

       [ProtoMember(24)]
        public int AdvertiserAccountId
        {
            get;
            set;
        }

       [ProtoMember(25)]
        public string AdvertiserName
        {
            get;
            set;
        }

       [ProtoMember(26)]
        public string AdvertiserAccountName
        {
            get;
            set;
        }
       [ProtoMember(27)]
        public bool IsSendNow
        {
            get;
            set;
        }

       [ProtoMember(28)]
        public int UserId
        {
            get;
            set;
        }

       [ProtoMember(29)]
        public bool IsPrimaryUser
        {
            get;
            set;
        }
       [ProtoMember(30)]
        public string ReportTitle { get; set; }
       [ProtoMember(31)]
        public bool? IsFinished { get; set; }
       [ProtoMember(32)]
        public DateTime? NextFireTime { get; set; }
       [ProtoMember(33)]
        public string LastRunningDateString
        {
            get;
            set;
        }

       [ProtoMember(34)]
        public string EndDateString
        {
            get;
            set;
        }
       [ProtoMember(35)]
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
       [ProtoMember(36)]
        public DateTime? LastRunningDate { get; set; }
       [ProtoMember(37)]
        public bool IsActive { get; set; }
       [ProtoMember(38)]
        public bool IsScheduled { get; set; }
       [ProtoMember(39)]
        public string ReportJsonCriteria { get; set; }
       [ProtoMember(40)]
        public DateTime? StartTime { get; set; }
       [ProtoMember(41)]
        public RecurrenceType RecurrenceType { get; set; }
       [ProtoMember(42)]
        public ReportSectionType ReportSectionType { get; set; }
       [ProtoMember(43)]
        public DateRecurrenceType DateRecurrenceType { get; set; }
       [ProtoMember(44)]
        public WeekDay WeekDay { get; set; }
       [ProtoMember(45)]
        public string DaysOfWeekParams { get; set; }

       [ProtoMember(46)]
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

       [ProtoMember(47)]
        public int MonthDay { get; set; }

       [ProtoMember(48)]
        public ReportCriteriaDto ReportDto { get; set; }

       [ProtoMember(49)]
        public int ReportId { get; set; }

       [ProtoMember(50)]
        public string URLLink { get; set; }
       [ProtoMember(51)]
        public string EmailIntroduction { get; set; }

       [ProtoMember(52)]
        public IList<int> MatixColumns { get; set; } = new List<int>();
        #region FiltersMembers

        [ProtoMember(53)]
        public DateTime FilterDateFrom { get; set; }
       [ProtoMember(54)]
        public DateTime FilterDateTo { get; set; }
       [ProtoMember(55)]
        public int FilterSize { get; set; }
       [ProtoMember(56)]
        public int FilterPage { get; set; }




        #endregion

        #region DaysMembers

       [ProtoMember(57)]
        public bool IsSunday { get; set; }
       [ProtoMember(58)]
        public bool IsSaturday { get; set; }
       [ProtoMember(59)]
        public bool IsFriday { get; set; }
       [ProtoMember(60)]
        public bool IsMonday { get; set; }


       [ProtoMember(61)]
        public bool IsTuesday { get; set; }
       [ProtoMember(62)]
        public bool IsWednesday { get; set; }

       [ProtoMember(63)]
        public bool IsThursday { get; set; }

        #endregion


       [ProtoMember(64)]
        public  bool IsForQueryBuilder { get; set; }

        public string StatusString
        {
            get
            {

                return this.NotActiveCondition ? "NotActive" : "Active";
            }
        }




        [ProtoMember(66)]
        public string ColorClassName
        {
            get

            {

                if (NextFireTime != null & NextFireTime >= Framework.Utilities.Environment.GetServerTime())
                {
                    if (IsFinished.HasValue)
                    {
                        if (IsFinished.Value & !this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "ReportStatusGreen";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate < NextFireTime)
                        {
                            return "ReportStatusRed";

                        }


                    }
                }

                if (NextFireTime != null & NextFireTime < Framework.Utilities.Environment.GetServerTime())
                {
                    if (IsFinished.HasValue)
                    {
                        if (!IsFinished.Value & !this.IsSendNow)
                        {
                            return "ReportStatusRed";
                        }
                        else if (!this.IsSendNow && this.LastRunningDate > NextFireTime)
                        {
                            return "ReportStatusGreen";

                        }
                    }
                    else if (!this.IsSendNow && this.LastRunningDate <= NextFireTime)
                    {
                        return "ReportStatusRed";

                    }
                }

                if (NextFireTime == null & IsFinished.HasValue)
                {
                    if (!IsFinished.Value & !this.IsSendNow)
                    {
                        return "ReportStatusRed";

                    }
                    if (IsFinished.Value & !this.IsSendNow)
                    {
                        return "ReportStatusGreen";

                    }
                }

                return "";
            }
            set { }
        }


    }

    [ProtoInclude(100,typeof(RecurrenceType))]
    [ProtoInclude(101,typeof(ReportSectionType))]
    [ProtoInclude(102,typeof(WeekDay))]
    [ProtoContract]
    public class testReportSchedulerDto
    {
       [ProtoMember(1)]
        public List<ReportRecipientDTO> AllReportRecipient { get; set; } = new List<ReportRecipientDTO>();
        [ProtoMember(2)]
        public string PreferedName { get; set; }
       [ProtoMember(3)]
        public string TriggerName { get; set; }
       [ProtoMember(4)]
        public string TriggerGroupName { get; set; }

       [ProtoMember(5)]
        public string Description { get; set; }
       [ProtoMember(6)]
        public int ID { get; set; }
       [ProtoMember(7)]
        public int AccountId { get; set; }
       [ProtoMember(8)]
        public int LastDocumnetGeneratedId { get; set; }
       [ProtoMember(9)]
        public string Name { get; set; }
       [ProtoMember(10)]
        public DateTime? EndDate { get; set; }
       [ProtoMember(11)]
        public DateTime StartDate { get; set; }
       [ProtoMember(12)]
        public DateTime CreationDate { get; set; }
       [ProtoMember(13)]
        public DateTime? TimeSentAt { get; set; }

       [ProtoMember(14)]
        public DateTime? LastRunningDate { get; set; }
       [ProtoMember(15)]
        public bool IsActive { get; set; }
       [ProtoMember(16)]
        public bool IsScheduled { get; set; }
       [ProtoMember(17)]
        public string ReportJsonCriteria { get; set; }
       [ProtoMember(18)]
        public DateTime? StartTime { get; set; }
       [ProtoMember(19)]
        public RecurrenceType RecurrenceType { get; set; }
       [ProtoMember(20)]
        public ReportSectionType ReportSectionType { get; set; }
       [ProtoMember(21)]
        public WeekDay WeekDay { get; set; }

       [ProtoMember(22)]
        public int MonthDay { get; set; }

    }


    [ProtoContract]
    public class ResultReportSchedulerDto
    {
       [ProtoMember(1)]
        public List<ReportSchedulerDto> Items { get; set; } = new List<ReportSchedulerDto>();
        [ProtoMember(2)]
        public List<ReportCriteriaSchedulerDto> CriteriaItems { get; set; } = new List<ReportCriteriaSchedulerDto>();
        [ProtoMember(3)]
        public long TotalCount { get; set; }

    }
}
