using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative
{
    [ProtoContract]
    public class AdGroupCostElementCriteria
    {
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public int AdGroupId { get; set; }
        [ProtoMember(3)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(4)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(5)]
        public int Page { get; set; }
        [ProtoMember(6)]
        public int Size { get; set; }
    }
}
