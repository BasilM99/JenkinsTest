using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class Advertiser : ManagedLookupBase
    {
        public virtual string UniqueId { get; set; }
        public virtual int AdvertiserBusinessId { get; set; }
     
        public virtual string Description { get; set; }

        public virtual string DomainURL { get; set; }
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
