using System;
using System.Security.Cryptography;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Noqoush.AdFalcon.Domain.Model.Account
{


    public class AccountPortalPermissions : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual int AccountId { get { if (Account != null) return Account.ID; else return 0; } set { } }

        public virtual Account Account  { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual PortalPermision Permission { get; set; }

        public virtual string GetDescription()
        {
            return Permission.Name.Value;
        }

    }
}
