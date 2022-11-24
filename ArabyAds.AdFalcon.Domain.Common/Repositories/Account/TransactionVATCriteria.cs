using System;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account
{
   
    [ProtoContract]
    public class TransactionVATCriteria 
    {
        [ProtoMember(1)]
        public int? AccountId { get; set; }
        [ProtoMember(2)]
        public int? UserId { get; set; }

        [ProtoMember(3)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(4)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(5)]
        public DateTime? DataTo { get; set; }

        [ProtoMember(6)]
        public int? Page { get; set; }
        [ProtoMember(7)]
        public int Size { get; set; }
        [ProtoMember(8)]
        public bool Details { get; set; }
        [ProtoMember(9)]
        public bool Payments { get; set; }
    }

}
