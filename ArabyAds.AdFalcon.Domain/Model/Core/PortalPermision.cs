//using ArabyAds.AdFalcon.Base;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework.DomainServices.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
    //[DataContract()]

    //public enum PortalPermissionsCode
    //{
    //    [EnumMember]
    //    PlainHTML = 1,
    //    [EnumMember]
    //    RichMedia = 3,
    //    [EnumMember]
    //    NativeAd = 4,
    //    [EnumMember]
    //    Interstitial = 2,
    //    [EnumMember]
    //    TrackingAd = 5,
    //    [EnumMember]
    //    InstreamVideo = 6,
    //    [EnumMember]
    //    PMPDeal = 7,
    //    [EnumMember]
    //    Audience = 8,
    //    [EnumMember]
    //    ReportSchedule = 9,
    //    [EnumMember]
    //    InventorySource = 10,
    //    [EnumMember]
    //    AudianceSegmentUsagePermission = 11,
    //    [EnumMember]
    //    TrafficPlanner = 15,

    //    [EnumMember]
    //    QueryBuilder = 16,

    //}
    public class PortalPermision : LookupBase<PortalPermision, int>
    {
        public virtual PortalPermissionsCode Code { get; set; }
        public virtual LocalizedString Categorie { get; set; }

    }
}
