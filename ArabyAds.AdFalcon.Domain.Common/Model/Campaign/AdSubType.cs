﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using System.Collections.Generic;


namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract()]
    public enum AdSubTypes
    {
        [EnumMember]
        [EnumText("ExpandableRichMedia", "Campaign")]
        ExpandableRichMedia = 1,
        [EnumMember]
        [EnumText("JavaScriptRichMedia", "Campaign")]
        JavaScriptRichMedia = 2,
        [EnumMember]
        [EnumText("JavaScriptInterstitial", "Campaign")]
        JavaScriptInterstitial = 3,
        [EnumMember]
        [EnumText("ExternalUrlInterstitial", "Campaign")]
        ExternalUrlInterstitial = 4,

        [EnumMember]
        [EnumText("VideoLinear", "Campaign")]
        VideoLinear = 5,
        [EnumMember]
        [EnumText("VideoEndCard", "Campaign")]
        VideoEndCard = 6,

        [EnumMember]
        [EnumText("HTML5RichMedia", "Campaign")]
        HTML5RichMedia = 7,

        [EnumMember]
        [EnumText("HTML5Interstitial", "Campaign")]
        HTML5Interstitial = 8,
    }

 

}
