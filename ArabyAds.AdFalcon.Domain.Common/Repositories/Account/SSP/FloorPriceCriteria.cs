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
    public class FloorPriceCriteria 
    {
        [ProtoMember(1)]
        public FloorPriceConfigType ConfigType { get; set; }
        [ProtoMember(2)]
        public int SiteId { get; set; }
        [ProtoMember(3)]
        public int ZoneId { get; set; }
        [ProtoMember(4)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(5)]
        public DateTime? DateTo { get; set; }

        [ProtoMember(6)]
        public int? Page { get; set; }
        [ProtoMember(7)]
        public int Size { get; set; }
        [ProtoMember(8)]
        public string Name { get; set; }

        private string ConvertToString(object String)
        {

            return String.ToString();

        }
  
    }
}
