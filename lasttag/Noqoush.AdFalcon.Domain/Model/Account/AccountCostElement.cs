﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using Noqoush.Framework.DomainServices;

using System.Security.Cryptography;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Domain.Model.Account
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


        public virtual Noqoush.AdFalcon.Domain.Model.Account.Account Account { get; set; }
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


        public virtual Noqoush.AdFalcon.Domain.Model.Account.Account Account { get; set; }
        public virtual Fee Fee { get; set; }



    }
}
