using Noqoush.AdFalcon.Domain.Common.Repositories;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core
{

    [DataContract(Name = "GroupByType")]
    public enum GroupByType
    {
        [EnumMember]
        ByCampaign = 1,
        [EnumMember]
        ByAdgroup = 2,
        [EnumMember]
        ByAd = 3


    }
    [DataContract(Name = "ReportSummaryBy")]
    public enum ReportSummaryBy
    {
        [EnumMember]
        Day = 1,
        [EnumMember]
        Week = 2,
        [EnumMember]
        Month = 3


    }

    [DataContract(Name = "RecurrenceType")]
    public enum RecurrenceType
    {
        [EnumMember]
        [EnumText("Daily", "Time")]
        Day = 1,
        [EnumMember]
        [EnumText("Weekly", "Time")]
        Week = 2,
        [EnumMember]
        [EnumText("Monthly", "Time")]
        Month = 3,
        [EnumText("0", "WeekDays")]
        [EnumMember]
        Year = 4

    }
    [DataContract(Name = "DateRecurrenceType")]
    public enum DateRecurrenceType
    {
        [EnumMember]
        Specific = 0,
        [EnumMember]
        Today = 1,
        [EnumMember]
        Last7Days = 2,
        [EnumMember]
        MonthtoDate = 3,
        [EnumMember]
        YearToDate = 4,
        [EnumMember]
        PreviousMonth = 5

    }

    [DataContract(Name = "WeekDay")]
    public enum WeekDay
    {
        [EnumText("0", "WeekDays")]
        [EnumMember]
        Sunday = 0,
        [EnumMember]
        [EnumText("1", "WeekDays")]
        Monday = 1,
        [EnumMember]
        [EnumText("2", "WeekDays")]
        Tuesday = 2,
        [EnumMember]
        [EnumText("3", "WeekDays")]
        Wednesday = 3,
        [EnumMember]
        [EnumText("4", "WeekDays")]
        Thursday = 4,
        [EnumMember]
        [EnumText("5", "WeekDays")]
        Friday = 5,
        [EnumMember]
        [EnumText("6", "WeekDays")]
        Saturday = 6
    }


    [DataContract(Name = "ReportSectionType")]
    public enum ReportSectionType
    {
        [EnumMember]
        Undefined = 0,
        [EnumMember]
        Publisher = 1,
        [EnumMember]
        Advertiser = 2,


    }
   /* [KnownType(typeof(RecurrenceType))]
    [KnownType(typeof(ReportSectionType))]
    [KnownType(typeof(WeekDay))]
    public class ReportScheduler : IEntity<int>
    {


        private IReportRecipientRepository _ReportRecipientRepository;
        private IAccountRepository _AccountRepository;


        private static IAdCreativeRepository _AdCreativeRepository = null;
        private static IAdCreativeRepository AdCreativeRepository
        {
            get
            {
                if (_AdCreativeRepository == null)
                {
                    _AdCreativeRepository = Framework.IoC.Instance.Resolve<IAdCreativeRepository>();
                }
                return _AdCreativeRepository;
            }
        }


        private static IAdGroupRepository _AdGroupRepository = null;
        private static IAdGroupRepository AdGroupRepository
        {
            get
            {
                if (_AdGroupRepository == null)
                {
                    _AdGroupRepository = Framework.IoC.Instance.Resolve<IAdGroupRepository>();
                }
                return _AdGroupRepository;
            }
        }


        private static IAppSiteRepository _appSiteRepository = null;
        private static IAppSiteRepository AppSiteRepository
        {
            get
            {
                if (_appSiteRepository == null)
                {
                    _appSiteRepository = Framework.IoC.Instance.Resolve<IAppSiteRepository>();
                }
                return _appSiteRepository;
            }
        }


        private static ICampaignRepository _CampaignRepository = null;
        private static ICampaignRepository CampaignRepository
        {
            get
            {
                if (_CampaignRepository == null)
                {
                    _CampaignRepository = Framework.IoC.Instance.Resolve<ICampaignRepository>();
                }
                return _CampaignRepository;
            }
        }
        public virtual IList<ReportRecipient> AllRecipient { get; set; }
        public virtual string PreferedName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string TriggerName { get; set; }
        public virtual string TriggerGroupName { get; set; }
        //public virtual string CompositeName { get; set; }
        //public virtual string CompositeEmail { get; set; }

        public virtual Account.Account Account { get; protected set; }


        public virtual Account.User User { get; set; }
        public virtual string JobName { get; set; }
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual DateTime? EndDate { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual DateTime? LastRunningDate { get; set; }
        public virtual DateTime? NextFireTime { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string EmailSubject { get; set; }
        public virtual Document LastDocumnetGenerated { get; set; }
        public virtual DateTime? TimeSentAt { get; set; }
        public virtual bool IsScheduled { get; set; }
        public virtual string DaysOfWeekParams { get; set; }
        public virtual ReportCriteria ReportCriteria { get; set; }

        public virtual string ReportJsonCriteria { get; set; }
        public virtual bool? IsFinished { get; set; }
        public virtual bool IsSendNow { get; set; }
        public virtual string GetDescription()
        {
            return ToString();
        }
        private string GetCampaignName(int campaignId)
        {

            var name = CampaignRepository.GetObjectName(campaignId);
            return name;
        }

        private string GetAdGroupName(int Id)
        {

            var name = AdGroupRepository.Query(M => M.ID == Id).Select(M => M.Campaign.Name).SingleOrDefault<string>(); ;
            return name;
        }
        private string GetAdName(int Id)
        {

            var name = AdCreativeRepository.Query(M => M.ID == Id).Select(M => M.Group.Campaign.Name).SingleOrDefault<string>(); ;
            return name;
        }
        private string GetAppSiteName(int appSiteId)
        {

            var name = AppSiteRepository.GetObjectName(appSiteId);
            return name;
        }
        public virtual string GetCompositeMame(string ItemsList, string TabId, bool isEmail)
        {
            string Name = "";

            Name = isEmail ? this.EmailSubject : this.Name;

            if (this.RecurrenceType == RecurrenceType.Day) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Daily", "Time"));
            if (this.RecurrenceType == RecurrenceType.Week) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Weekly", "Time"));
            if (this.RecurrenceType == RecurrenceType.Month) Name = Name.Replace("{Recurrence}", ResourceManager.Instance.GetResource("Monthly", "Time"));
            string substitutionName = string.Empty;
            List<int> Ids = new List<int>();

            int tempId = 0;

            if (!string.IsNullOrEmpty(ItemsList))
            {
                string AdsList = ItemsList.Trim(new char[] { ',' });
                var arrString = AdsList.Split(new char[] { ',' });
                foreach (var id in arrString)
                {
                    tempId = 0;
                    int.TryParse(id, out tempId);
                    if (tempId > 0)
                        Ids.Add(tempId);
                }
            }
            if (Ids.Count == 1 && string.IsNullOrEmpty(this.PreferedName))
            {
                switch (TabId.ToLower())
                {
                    case "campaign":
                        substitutionName = GetCampaignName(Ids[0]);
                        Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("CampaignSubject", "Report") + substitutionName);
                        break;
                    case "adgroup":
                        substitutionName = GetAdGroupName(Ids[0]);
                        Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AdGroupSubject", "Report") + substitutionName);
                        break;
                    case "ad":
                        substitutionName = GetAdName(Ids[0]);
                        Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AdSubject", "Report") + substitutionName);
                        break;
                    case "app":
                        substitutionName = GetAppSiteName(Ids[0]);
                        Name = Name.Replace("{EntityName}", ResourceManager.Instance.GetResource("AppSiteSubject", "Report") + substitutionName);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(this.PreferedName))
            {
                substitutionName = this.PreferedName;
                Name = Name.Replace("{EntityName}", this.PreferedName);

            }
            if (string.IsNullOrEmpty(substitutionName))
            {
                substitutionName = DateTime.Now.ToString("yyyyMMdd");

                Name = Name.Replace(" - {EntityName}", string.Empty);
                Name = Name.Replace("{EntityName}", string.Empty);
            }

            Name = Name.Replace("{Date}", String.Format("{0:yyyyMMdd}", this.CreationDate));
            Name = Name.Replace("{ReportId}", this.ReportId.ToString());
            return Name;

        }
        public virtual string GetDaysOfWeekDescription(string DaysOfWeekParams)
        {

            string ressult = string.Empty;
            if (DaysOfWeekParams != null)
            {
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Friday))
                {
                    ressult = ressult + DayOfWeek.Friday.ToText() + ",";

                }
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Saturday))
                {
                    ressult = ressult + DayOfWeek.Saturday.ToText() + ",";

                }
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Sunday))
                {
                    ressult = ressult + DayOfWeek.Sunday.ToText() + ",";

                }

                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Monday))
                {
                    ressult = ressult + DayOfWeek.Monday.ToText() + ",";

                }
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Tuesday))
                {
                    ressult = ressult + DayOfWeek.Tuesday.ToText() + ",";

                }
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Wednesday))
                {
                    ressult = ressult + DayOfWeek.Wednesday.ToText() + ",";


                }
                if (DaysOfWeekParams.Contains("" + (int)DayOfWeek.Thursday))
                {
                    ressult = ressult + DayOfWeek.Thursday.ToText() + ",";

                }
            }
            return ressult;
        }
        public override string ToString()
        {
            return this.Name;
        }

        public virtual void SendNow()
        {
            this.IsSendNow = true;
            //this.IsActive = true;
            //this.IsScheduled = false;
            this.IsFinished = false;
        }
        public virtual void Resume()
        {
            this.IsActive = true;
            this.IsScheduled = false;
        }
        public virtual void Pause()
        {
            this.IsActive = false;
            this.IsScheduled = false;
        }
        public virtual void Delete()
        {
            this.IsActive = false;
            this.IsScheduled = false;
            this.IsDeleted = true;
        }
        public virtual RecurrenceType RecurrenceType { get; set; }
        public virtual ReportSectionType ReportSectionType { get; set; }
        public virtual WeekDay WeekDay { get; set; }
        public virtual DateRecurrenceType DateRecurrenceType { get; set; }
        public virtual int ReportId { get; set; }
        public virtual int MonthDay { get; set; }
        public virtual string EmailIntroduction { get; set; }
        public virtual bool IsForQueryBuilder { get; set; }
    }*/
}
