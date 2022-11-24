
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


namespace ArabyAds.AdFalcon.Domain.Model.Account
{
   
    public class AccountPartyDefine : IEntity<int>
    {

        public virtual string Description { get; set; }

        public virtual int ID
        {
            get;
            protected set;
        }
        public virtual bool IsDeleted
        {
            get;
             set;
        }
        public virtual Account Account
        {
            get;
             set;
        }
        public virtual Party Party
        {
            get;
             set;
        }
        public virtual string GetDescription()
        {
            return this.ID.ToString();
        }
    
    }
}
