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
    public class SiteZoneCriteria    {
        [ProtoMember(1)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(2)]
        public DateTime? DateTo { get; set; }

        [ProtoMember(3)]
        public int BusinessId { get; set; }
        [ProtoMember(4)]
        public int SiteId { get; set; }

        [ProtoMember(5)]
        public int? Page { get; set; }
        [ProtoMember(6)]
        public int Size { get; set; }
        [ProtoMember(7)]
        public string ZoneName { get; set; }
        [ProtoMember(8)]
        public string ZoneId { get; set; }

    }
}
