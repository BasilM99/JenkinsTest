using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

using System.Linq;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Account.SSP;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account.SSP
{

    [ProtoContract]
    public class SiteZoneMappingCriteria 
    {
        [ProtoMember(1)]
        public string AdFalconSubPublisherId { get; set; }
        [ProtoMember(2)]
        public string AppSiteName { get; set; }
        [ProtoMember(3)]
        public int? AdTypeId { get; set; }
        [ProtoMember(4)]
        public int? DeviceTypeId { get; set; }
        [ProtoMember(5)]
        public int AppSiteId { get; set; }
        [ProtoMember(6)]
        public bool? IsInterstitial { get; set; }
        [ProtoMember(7)]
        public int ZoneId { get; set; }
        [ProtoMember(8)]
        public int SiteId { get; set; }
        [ProtoMember(9)]
        public int BusinessId { get; set; }
        [ProtoMember(10)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(11)]
        public DateTime? DateTo { get; set; }

        [ProtoMember(12)]
        public int? Page { get; set; }
        [ProtoMember(13)]
        public int Size { get; set; }
        [ProtoMember(14)]
        public string MappingName { set; get; }

    }
}
