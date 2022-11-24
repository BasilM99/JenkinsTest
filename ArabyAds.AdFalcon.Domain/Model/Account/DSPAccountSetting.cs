using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    //[DataContract(Name = "AgencyCommission")]
    //public enum AgencyCommission
    //{
    //    [EnumMember]
    //    [EnumText("Undefined", "SubAppSite")]
    //    Undefined = 0,
    //    [EnumMember]
    //    [EnumText("GrossCostMargin", "Global")]
    //    FixedCPM = 1,
    //    [EnumMember]
    //    [EnumText("NetCostMargin", "Global")]
    //    NetCostMargin = 2,
    //    [EnumMember]
    //    [EnumText("BillableCostMargin", "Global")]
    //    BillableCostMargin = 3,
    //    [EnumMember]
    //    [EnumText("GrossCostMargin", "Global")]
    //    GrossCostMargin = 4
    //}
    public class DSPAccountSetting : IEntity<int>
    {
        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual string BusinessName { get; set; }
        public virtual string BillingContactName { get; set; }
        public virtual int ID { get;  set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual AgencyCommission AgencyCommission { get; set; }
        public virtual Account Account { get; set; }
        public virtual IList<DSPAccountSettingContact> Contacts { get; set; }
        public virtual string BillToAddressPersonName { get; set; }
        public virtual string BillToAddress1 { get; set; }
        public virtual string BillToAddress2 { get; set; }
        public virtual string GetDescription()
        {

            return BusinessName;


        }
    }


    public class DSPAccountSettingContact : IEntity<int>
    {
        public virtual int ID { get; protected set; }

        public virtual bool IsDeleted { get; set; }

        public virtual string Email { get; set; }

        public virtual DSPAccountSetting DSPAccountSetting { get; set; }

        public virtual string GetDescription()
        {
            return this.Email;
        }
    }
}
