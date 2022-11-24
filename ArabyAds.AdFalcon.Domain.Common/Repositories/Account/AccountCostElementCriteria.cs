using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Account
{
    [ProtoContract]
    public class AccountCostElementCriteria 
    {
        [ProtoMember(1)]
        public int AccountId { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public int? Page { get; set; }
        [ProtoMember(4)]
        public int Size { get; set; }

    }

    [ProtoContract]
    public class AccountFeeCriteria
    {

        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int? Page { get; set; }
        [ProtoMember(3)]
        public int Size { get; set; }
        [ProtoMember(4)]
        public int AccountId { get; set; }

    }
}
