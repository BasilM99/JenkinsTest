using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign
{
    [ProtoContract]
    public class CampaignCriteria 
    {
        [ProtoMember(1)]
        public int AccountId { get; set; }

        [ProtoMember(2)]
        public int? userId { get; set; }
        [ProtoMember(3)]
        public bool ActiveCampaigns { get; set; }
        [ProtoMember(4)]
        public bool IsPrimaryUser { get; set; }
        [ProtoMember(5)]
        public DateTime? DataCreate { get; set; }
        [ProtoMember(6)]
        public DateTime? DataFrom { get; set; }
        [ProtoMember(7)]
        public DateTime? DataTo { get; set; }
        [ProtoMember(8)]
        public CampaignType CampaignType { get; set; }
        [ProtoMember(9)]
        public CampaignType OtherCampaignType { get; set; }
        //public int? StatusId { get; set; }
        [ProtoMember(10)]
        public int? Page { get; set; }
        [ProtoMember(11)]
        public int Size { get; set; }
        [ProtoMember(12)]
        public int? AppSiteId { get; set; }
        [ProtoMember(13)]
        public int? AdvertiserAccountId { get; set; }
        [ProtoMember(14)]
        public int? AdvertiserId { get; set; }
        [ProtoMember(15)]
        public string Name { get; set; }
        public CampaignCriteria()
        {
            CampaignType = CampaignType.Normal;
            OtherCampaignType = CampaignType.ProgrammaticGuaranteed;
        }
     
    }
    [ProtoContract]
    public class AllCampaignCriteria 
    {
        [ProtoMember(1)]
        public int? AppSiteId { get; set; }
        public AllCampaignCriteria()
        {
        }
   
    }
}
