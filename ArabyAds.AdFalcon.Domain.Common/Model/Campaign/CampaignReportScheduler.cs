using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign
{

    //[DataContract(Name = "GroupByType")]
    //public enum GroupByType
    //{
    //    [EnumMember]
    //    ByCampaign = 1,
    //    [EnumMember]
    //    ByAdgroup = 2,
    //    [EnumMember]
    //    ByAd = 3


    //}
    //[DataContract(Name = "ReportSummaryBy")]
    //public enum ReportSummaryBy
    //{
    //    [EnumMember]
    //    Day = 1,
    //    [EnumMember]
    //    Week = 2,
    //    [EnumMember]
    //    Month = 3


    //}


   /* public class CampaignReportScheduler : IEntity<int>
    {
        public virtual IList<CampaignReportRecipient> AllRecipient { get; set; }
        public virtual ReportScheduler ReportScheduler { get; set; }
        public virtual int ID { get; private set; }
        public virtual bool IsDeleted { get; set; }
        public virtual IEnumerable<Campaign> Campaigns { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual string PreferedName { get; set; }
        public virtual string GetDescription()
        {
            return ToString();
        }
        public override string ToString()
        {
            return PreferedName;
        }
        public virtual GroupByType GroupByType { get; set; }
        public virtual ReportSummaryBy ReportSummaryBy { get; set; }
        public virtual void delete()
        {
            this.ReportScheduler.IsDeleted = true;
            this.IsDeleted = true;

        }
    }*/
}
