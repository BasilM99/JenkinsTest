
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using Noqoush.Framework.DomainServices;


namespace Noqoush.AdFalcon.Domain.Model.Account
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
