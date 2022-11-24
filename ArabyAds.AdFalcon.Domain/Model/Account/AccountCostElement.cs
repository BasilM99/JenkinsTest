using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Resources;
using ArabyAds.Framework.DomainServices;

using System.Security.Cryptography;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;

namespace ArabyAds.AdFalcon.Domain.Model.Account
{

    public class AccountCostElement : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }
        public virtual int ID { get; set; }
        public virtual Party Beneficiary { get; set; }
        public virtual decimal CostValue { get; set; }
        
        public virtual bool Enabled { get; set; }
        public virtual string GetDescription()
        {
            return this.Account.GetDescription() + "-" + this.CostElement.GetDescription();
        }


        public virtual ArabyAds.AdFalcon.Domain.Model.Account.Account Account { get; set; }
        public virtual CostElement CostElement { get; set; }

        public virtual DPPartner DataProvider { get; set; }

    }


    public class AccountFee : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }
        public virtual int ID { get; set; }
        public virtual Party Beneficiary { get; set; }
        public virtual decimal CostValue { get; set; }

        public virtual bool Enabled { get; set; }
        public virtual string GetDescription()
        {
            return this.Account.GetDescription() + "-" + this.Fee.GetDescription();
        }


        public virtual ArabyAds.AdFalcon.Domain.Model.Account.Account Account { get; set; }
        public virtual Fee Fee { get; set; }



    }
}
