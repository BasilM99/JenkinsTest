using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
{
    [ProtoContract]
    public class DocumentCriteria
    {

    }
    [ProtoContract]
    public class KeywordCriteria 
    {
        [ProtoMember(1)]
        public string Value { get; set; }
    }
}
