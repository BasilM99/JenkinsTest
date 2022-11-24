
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AccountN = Noqoush.AdFalcon.Domain.Common.Model.Account;


namespace Noqoush.AdFalcon.Domain.Common.Model.Account
{

    [DataContract(Name = "UserType")]
    public enum UserType
    {
        [EnumMember]
        [EnumText("Undefined", "AccountDSPRequest")]
        Undefined =0,
        [EnumMember]
        [EnumText("Normal", "UserType")]
        Normal = 1,
        [EnumMember]
        [EnumText("ReadOnly", "UserType")]
        ReadOnly = 2,
        [EnumMember]
        [EnumText("Primary", "UserType")]
        Primary = 3,
  
    }
    /*
    public class UserAccounts
    {
        public virtual int ID { get; set; }
        public virtual User User { get; set; }
        public virtual Account Account { get; set; }
        public virtual bool IsSecondPrimaryUser { get; set; }

        public virtual UserType UserType { get; set; }
    }*/
}
