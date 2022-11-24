﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;


using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Targeting;
using System.Linq;

using ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign;
using System.Threading;
using ArabyAds.AdFalcon.Domain.Common.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Common.Repositories;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract(Name = "CampaignType")]
    public enum CampaignType
    {
        [EnumMember]
        [EnumText("Undefined", "BidConfigType")]
        Undefined = 0,
        [EnumMember]
        [EnumText("NormalAd", "Campaign")]
        Normal = 1,
        [EnumText("AdHouse", "Campaign")]
        [EnumMember]
        AdHouse = 2,
        [EnumMember]
        ProgrammaticGuaranteed =3


    }

    [DataContract(Name = "PriceMode")]
    public enum PriceMode
    {
        [EnumMember]
        [EnumText("Undefined", "BidConfigType")]
        Undefined = 0,
        [EnumMember]
        [EnumText("FixedType", "CampaignSettings")]
        Fixed = 1,
        [EnumText("Dynamic", "Global")]
        [EnumMember]
        Dynamic = 2
    }

    [DataContract(Name = "CampaignLifeTime")]
    public enum CampaignLifeTime
    {
        [EnumMember]
        [EnumText("Undefined", "BidConfigType")]
        Undefined = 0,
        [EnumMember]
        [EnumText("Default", "Global")]
        Default = 1,
        [EnumText("Dynamic", "Global")]
        [EnumMember]
        LifeTime = 2
    }
    
}
