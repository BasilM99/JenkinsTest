using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{

    //[DataContract(Name = "ReportCriteriaScope")]
    //public enum ReportCriteriaScope
    //{
    //    [EnumMember]
    //    User = 0,
    //    [EnumMember]
    //    System = 1,
    //    [EnumMember]
    //    Admin = 2,


    //}
    public class ReportCriteria : IEntity<int>
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
    }


    public class DashBoardCriteria : ReportCriteria
    {
        public virtual bool ForDashBoard { get; set; }
    }
}
