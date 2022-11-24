using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign
{


    [DataContract()]
    public enum ClickMethod
    {
        [EnumMember]
        [EnumText("SubAppSite", "Undefined")]
        Undefined  = 0,
        [EnumMember]
        [EnumText("EntireAdClickable", "Global")]
        EntireAdClickable = 1,
        [EnumMember]
        [EnumText("QueryStringParameter", "Global")]
        QueryStringParameter = 2,
        [EnumMember]
        [EnumText("QueryStringParameterRe", "Global")]
        QueryStringParameterRedirectOnly = 3,

       
    }
    [DataContract()]
    public enum EnvironmentType
    {
        [EnumMember]
        [EnumText("All", "Global")]
        All = 0,
        [EnumMember]
        [EnumText("WebEnvironmentType", "Campaign")]
        Web = 1,
        [EnumMember]
        [EnumText("AppEnvironmentType", "Campaign")]
        App = 2
    }
    [DataContract()]
    public enum OrientationType
    {
        [EnumMember]
        [EnumText("Any", "Global")]
        Any = 0,
        [EnumMember]
        [EnumText("PortraitOrientationType", "Campaign")]
        Portrait = 1,
        [EnumMember]
        [EnumText("LandscapeOrientationType", "Campaign")]
        Landscape = 2
    }

}
