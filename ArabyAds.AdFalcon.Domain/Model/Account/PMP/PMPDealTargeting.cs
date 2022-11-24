using ArabyAds.AdFalcon.Domain.Common.Model.Account.PMP;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Account.PMP
{
    //[DataContract(Name = "PMPDealTargetingType")]
    //public enum PMPDealTargetingType
    //{
    //    [EnumMember]

    //    Undefined = 0,
    //    [EnumMember]
    //    [EnumText("AdFormat", "PMPDealTargetings")]
    //    AdFormat = 1,

    //    [EnumMember]
    //    [EnumText("AdSize", "PMPDealTargetings")]
    //    AdSize = 2,
    //    [EnumMember]
    //    [EnumText("Location", "PMPDealTargetings")]
    //    Location = 3
    //}
    public class PMPDealTargeting : IEntity<int>
    {
       
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual PMPDeal Deal { get; set; }
        public virtual PMPDealTargetingType Type { get; set; }

    

        public virtual string GetDescription()
        {
            return Type.ToText();
        }
      

    }
}
