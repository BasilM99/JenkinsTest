﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Runtime.Serialization;
//using ArabyAds.AdFalcon.Base;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices.Localization;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign.Objective
{
    //[DataContract]
    //public enum AdGroupObjectiveTypeIds
    //{
    //    [EnumMember]
    //    PromoteAppSite=1,
    //    [EnumMember]
    //    GenerateLead = 2,
    //    [EnumMember]
    //    PromoteContent = 3,
    //    [EnumMember]
    //    DriveTraffic = 4,
    //    [EnumMember]
    //    GenerateAwareness = 5
    //}
    public class AdGroupObjectiveType : LookupBase<AdGroupObjectiveType, int>
    {
        public virtual LocalizedString Descrption
        {
            get;
            set;
        }

        public virtual IEnumerable<AdActionTypeBase> AdActionTypes
        {
            get;
            set;
        }
    }
}

