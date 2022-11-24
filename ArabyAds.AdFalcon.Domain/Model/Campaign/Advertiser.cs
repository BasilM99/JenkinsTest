using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Tenant;
using ArabyAds.Framework;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class Advertiser : ManagedLookupBase , ITenant<int>
    {
        public virtual string UniqueId { get; set; }
        public virtual int AdvertiserBusinessId { get; set; }
     
        public virtual string Description { get; set; }

        public virtual string DomainURL { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

    //[DataContract()]
    //public enum TrackingPixel
    //{
    //    [EnumMember]
    //    [EnumText("ImageURL", "Pixel")]
    //    ImageURL = 0,
    //    [EnumMember]
    //    [EnumText("MobileTrackingURLAND", "Pixel")]
    //    MobileTrackingURLAND = 1,
    //    [EnumText("MobileTrackingURLIOS", "Pixel")]
    //    MobileTrackingURLIOS = 2,
    //    /*
    //    [EnumText("RedirectURL", "Pixel")]
    //    [EnumMember]
    //    RedirectURL = 3,*/
    //    [EnumText("ScriptTag", "Pixel")]
    //    [EnumMember]
    //    ScriptTag = 4,
    //    [EnumText("ScriptTagHTTPS", "Pixel")]
    //    [EnumMember]
    //    ScriptHTTPsTag = 5,



    //}
}
