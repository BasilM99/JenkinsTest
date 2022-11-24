using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative
{
    [ProtoContract]
    public class AdRequestCriteria
    {
        [ProtoMember(1)]
        public int AdGroupId { get; set; }
        [ProtoMember(2)]
        public int Page { get; set; }
        [ProtoMember(3)]
        public int Size { get; set; }

    }

    [ProtoContract]
    public class ImpressionMetricCriteria
    {
        [ProtoMember(1)]
        public int AdGroupId { get; set; }
        [ProtoMember(2)]
        public int Page { get; set; }
        [ProtoMember(3)]
        public int Size { get; set; }

    }
}
