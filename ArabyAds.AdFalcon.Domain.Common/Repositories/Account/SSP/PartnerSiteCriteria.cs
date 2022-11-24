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
    public class PartnerSiteCriteria 
    {
        [ProtoMember(1)]
        public int PartnerId { get; set; }
        [ProtoMember(2)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(3)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(4)]
        public int? Page { get; set; }
        [ProtoMember(5)]
        public int Size { get; set; }
        [ProtoMember(6)]
        public string SiteName { get; set; }
        [ProtoMember(7)]
        public string SiteId { get; set; }

    }
}
