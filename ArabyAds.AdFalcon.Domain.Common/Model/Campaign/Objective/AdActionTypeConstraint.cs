﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Objective
{
    public class AdActionTypeConstraint : LookupBase<AdActionTypeConstraint, int>
    {
        public virtual AdActionTypeBase AdActionType
        {
            get;
            set;
        }
        public virtual Platform Platform
        {
            get;
            set;
        }
        public virtual int DeviceConstraint
        {
            get;
            set;
        }
    }
}

