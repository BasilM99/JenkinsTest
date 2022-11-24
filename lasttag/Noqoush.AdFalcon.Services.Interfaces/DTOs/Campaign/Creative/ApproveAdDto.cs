using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class ApproveAdDto
    {
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public int GroupId { get; set; }
        [DataMember]
        public int AdId { get; set; }
        [DataMember]
        public int[] AppSiteIds { get; set; }
        [DataMember]
        public int[] DeletedAppSiteIds { get; set; }
        [DataMember]
        public string RunType { get; set; }
        [DataMember]
        public bool Include { get; set; }
        [DataMember]
        public string DomainURL { get; set; }
        [DataMember]
        public int? KeywordId { get; set; }

        [DataMember]
        public int? LanguageId { get; set; }


        [DataMember]
        public int[] AdsToCopyAppSites { get; set; }

        [DataMember]
        public List<AdCreativeUnitDto> Snapshots { get; set; }

        [DataMember]
        public List<AdCreativeUnitDto> AdCreativesAttribues { get; set; }

        [DataMember]
        public IList<CampaignBidConfigDto> UpdatedCampaignBidConfigDtos { get; set; }

    }
}
