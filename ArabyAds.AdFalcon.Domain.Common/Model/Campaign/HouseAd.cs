﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract()]
    public enum HouseAdDeliveryMode
    {
        [EnumMember]
        [EnumText("WhenNoAds", "HouseAd")]
        WhenNoAds = 1,
        [EnumMember]
        [EnumText("FullyAllocate", "HouseAd")]
        FullyAllocate = 2
    }
 
}
