using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Core
{
    [DataContract(Name = "KPIScope")]
    public enum KPIScope
    {
        [EnumMember]
        undefined = 0,
        [EnumMember]
        Advertiser = 1,
        [EnumMember]
        Deals = 2,
        [EnumMember]
        Publisher = 3,
        [EnumMember]
        Campaign = 4,
        [EnumMember]
        DataProvider = 5,
        [EnumMember]
        AdGroup = 6,
        [EnumMember]
        Ads = 7,
    }


    [DataContract(Name = "ReportCriteriaScope")]
    public enum ReportCriteriaScope
    {
        [EnumMember]
        User = 0,
        [EnumMember]
        System = 1,
        [EnumMember]
        Admin = 2,


    }
    /*public class ReportCriteria : IEntity<int>
    {
        public virtual string Name { get; set; }
        public virtual int ID { get; set; }
        public virtual User User { get; set; }
        public virtual Account.Account Account { get; set; }
        public virtual string Criteria { get; set; }
        public virtual ReportCriteriaScope ReportScope { get; set; }
        public virtual ReportSectionType SectionType { get; set; }

        public virtual IList<metriceColumnReportCriteria> Columns { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual string GetDescription()
        {
            return ToString();
        }
    }*/
}
