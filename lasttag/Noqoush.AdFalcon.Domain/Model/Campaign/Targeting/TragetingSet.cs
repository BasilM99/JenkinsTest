﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class TargetingSet : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual IEnumerable<TargetingBase> Targetings
        {
            get;
            set;
        }
        public virtual bool IsMatch(AdRequestTargeting adRequest)
        {
            throw new System.NotImplementedException();
        }
        public virtual string GetDescription()
        {
            return ToString();
        }
    }
}
