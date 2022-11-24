using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{
    public class AccountAPIAccess : IEntity<int>
    {
        public AccountAPIAccess() { }

        public AccountAPIAccess(Account account)
        {
            this.Account = account;
        }

        public virtual bool IsDeleted
        {
            get;
            set;
        }

        public virtual int ID { get; protected  set; }

        public virtual int AccountId { get; set; }
        public virtual string APIClientId { get; set; }
        public virtual string APISecretKey { get; set; }
        public virtual Account Account { get; set; }

        public virtual string GetDescription()
        {
            return string.Format(Framework.Resources.ResourceManager.Instance.GetResource("APIClientId", "APIAccess")+ ": {0}", APIClientId);
        }
    }
}
