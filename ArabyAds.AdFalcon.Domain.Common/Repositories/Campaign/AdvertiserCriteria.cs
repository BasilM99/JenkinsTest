using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign
{
   [ProtoContract]
    public class AdvertiserCriteria 
    {
        [ProtoMember(1)]
        public string Value { get; set; }
        [ProtoMember(2)]
        public string Culture { get; set; }
        [ProtoMember(3)]
        public int? Page { get; set; }
        [ProtoMember(4)]
        public int Size { get; set; }


    }




   [ProtoContract]
    public class AdvertiserAccountCriteria
    {
        [ProtoMember(1)]
        public bool IsReadOnly  { get; set; }
        [ProtoMember(2)]
        public int AccountId { get; set; }
        [ProtoMember(3)]
        public string culture { get; set; }
        [ProtoMember(4)]
        public int? userId { get; set; }
        [ProtoMember(5)]
        public bool showActive { get; set; }
        [ProtoMember(6)]
        public bool showArchived { get; set; }
        [ProtoMember(7)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(8)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(9)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(10)]
        public int? Page { get; set; }
        [ProtoMember(11)]
        public int Size { get; set; }

        [ProtoMember(12)]
        public string Name { get; set; }

    }
}
