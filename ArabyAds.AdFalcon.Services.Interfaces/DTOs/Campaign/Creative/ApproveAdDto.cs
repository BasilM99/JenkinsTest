using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class ApproveAdDto
    {
       [ProtoMember(1)]
        public int CampaignId { get; set; }
       [ProtoMember(2)]
        public int GroupId { get; set; }
       [ProtoMember(3)]
        public int AdId { get; set; }
       [ProtoMember(4)]
        public int[] AppSiteIds { get; set; }
       [ProtoMember(5)]
        public int[] DeletedAppSiteIds { get; set; }
       [ProtoMember(6)]
        public string RunType { get; set; }
       [ProtoMember(7)]
        public bool Include { get; set; }
       [ProtoMember(8)]
        public string DomainURL { get; set; }
       [ProtoMember(9)]
        public int? KeywordId { get; set; }

       [ProtoMember(10)]
        public int? LanguageId { get; set; }


       [ProtoMember(11)]
        public int[] AdsToCopyAppSites { get; set; }

       [ProtoMember(12)]
        public List<AdCreativeUnitDto> Snapshots { get; set; }

       [ProtoMember(13)]
        public List<AdCreativeUnitDto> AdCreativesAttribues { get; set; }

       [ProtoMember(14)]
        public IList<CampaignBidConfigDto> UpdatedCampaignBidConfigDtos { get; set; }

    }
}
