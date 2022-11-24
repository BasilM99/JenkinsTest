using System;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
{
    [ProtoContract]
    public class PartyCriteria 
    {
        [ProtoMember(1)]
        public List<int> notInclud { get; set; }
        [ProtoMember(2)]
        public PartyType? Type { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public int? Page { get; set; }
        [ProtoMember(5)]
        public int Size { get; set; }
        [ProtoMember(6)]
        public string Code { get; set; }
        [ProtoMember(7)]
        public bool Visible { get; set; }
        [ProtoMember(8)]
        public bool ShowArchive { get; set; }

        [ProtoMember(9)]
        public bool NotType { get; set; }

    }
    [ProtoContract]
    public class DPPartnerCriteria 
    {
        [ProtoMember(1)]
        public List<int> notInclud { get; set; }
        [ProtoMember(2)]
        public PartyType? Type { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public int? Page { get; set; }
        [ProtoMember(5)]
        public int Size { get; set; }
        [ProtoMember(6)]
        public string Code { get; set; }
        [ProtoMember(7)]
        public bool Visible { get; set; }



    }

}
