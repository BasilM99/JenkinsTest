﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using System.Collections.Generic;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{


    public class SubtypeAction : IEntity<int>
    {

        public virtual int ID { set; get; }

        public virtual AdSubType SubType { set; get; }

        public virtual AdActionTypeBase ActionType { set; get; }

        public virtual bool IsDeleted { set; get; }

        public virtual string GetDescription()
        {
            return "";

        }

    }

}