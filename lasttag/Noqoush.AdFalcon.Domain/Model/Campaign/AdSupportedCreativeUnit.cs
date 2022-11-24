using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{


    //[DataContract()]
    //public enum ClickMethod
    //{
    //    [EnumMember]
    //    [EnumText("SubAppSite", "Undefined")]
    //    Undefined  = 0,
    //    [EnumMember]
    //    [EnumText("EntireAdClickable", "Global")]
    //    EntireAdClickable = 1,
    //    [EnumMember]
    //    [EnumText("QueryStringParameter", "Global")]
    //    QueryStringParameter = 2,
    //    [EnumMember]
    //    [EnumText("QueryStringParameterRe", "Global")]
    //    QueryStringParameterRedirectOnly = 3,

       
    //}
    //[DataContract()]
    //public enum EnvironmentType
    //{
    //    [EnumMember]
    //    [EnumText("All", "Global")]
    //    All = 0,
    //    [EnumMember]
    //    [EnumText("WebEnvironmentType", "Campaign")]
    //    Web = 1,
    //    [EnumMember]
    //    [EnumText("AppEnvironmentType", "Campaign")]
    //    App = 2
    //}
    //[DataContract()]
    //public enum OrientationType
    //{
    //    [EnumMember]
    //    [EnumText("Any", "Global")]
    //    Any = 0,
    //    [EnumMember]
    //    [EnumText("PortraitOrientationType", "Campaign")]
    //    Portrait = 1,
    //    [EnumMember]
    //    [EnumText("LandscapeOrientationType", "Campaign")]
    //    Landscape = 2
    //}
    public class AdSupportedCreativeUnit : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual AdType AdType { get; set; }
        public virtual AdSubTypes? AdSubType { get; set; }
        public virtual EnvironmentType EnvironmentType { get; set; }
        public virtual RequiredType RequiredType { get; set; }
        public virtual CreativeUnit CreativeUnit { get; set; }
        public virtual AdSupportedCreativeUnit OrientationReplacement { get; set; }

        public virtual string GetDescription()
        {
            return CreativeUnit.GetDescription() + " " + AdType.GetDescription();
        }

        public virtual bool IsDeleted { get; set; }
    }
}
