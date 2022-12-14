using System;
using System.Security.Cryptography;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ArabyAds.AdFalcon.Domain.Model.Account
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
