using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative
{
    [ProtoContract]
    public class AdsCriteria 
    {
        public AdsCriteria()
        {
            CampaignType = CampaignType.Normal;
            CampaignOtherType = CampaignType.ProgrammaticGuaranteed;
        }
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public int GroupId { get; set; }
        [ProtoMember(3)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(4)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(5)]
        public int? StatusId { get; set; }
        [ProtoMember(6)]
        public int Page { get; set; }
        [ProtoMember(7)]
        public string Name { get; set; }

        [ProtoMember(8)]
        public int Size { get; set; }
        [ProtoMember(9)]
        public List<int> Permissions
        {
            get;
            set;
        }

        [ProtoMember(10)]
        public CampaignType CampaignType { get; set; }
        [ProtoMember(11)]
        public CampaignType CampaignOtherType { get; set; }
    }
    [ProtoContract]
    public class AdsSummaryCriteria 
    {
        [ProtoMember(1)]
        public string AccountName { get; set; }
        [ProtoMember(2)]
        public string CampaignName { get; set; }
        [ProtoMember(3)]
        public string CompanyName { get; set; }
        [ProtoMember(4)]
        public int CampaignId { get; set; }
        [ProtoMember(5)]
        public int GroupId { get; set; }
        [ProtoMember(6)]
        public int? StatusId { get; set; }
        [ProtoMember(7)]
        public int? Account { get; set; }
        [ProtoMember(8)]
        public DateTime? DateFrom { get; set; }
        [ProtoMember(9)]
        public DateTime? DateTo { get; set; }
        [ProtoMember(10)]
        public int Page { get; set; }
        [ProtoMember(11)]
        public int Size { get; set; }

    }
}
