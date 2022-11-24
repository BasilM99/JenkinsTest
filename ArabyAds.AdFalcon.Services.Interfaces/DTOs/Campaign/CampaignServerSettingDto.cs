using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class CampaignServerSettingDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }

       [ProtoMember(2)]
        public virtual string Name { get; set; }

       [ProtoMember(3)]
        [Range(1, 2592000)]
        public int? AdRequestCacheLifeTime { get; set; }

       [ProtoMember(4)]
        public virtual IList<CampaignFrequencyCappingDto> FrequencyCappingList { get; set; } = new List<CampaignFrequencyCappingDto>();

        [ProtoMember(5)]
        public virtual IEnumerable<CampaignFrequencyCappingDto> DefultFrequencyCappingList { get; set; } = new List<CampaignFrequencyCappingDto>();
        [ProtoMember(6)]
      //  public AgencyCommission AgencyCommission { get; set; }
        public int AgencyCommission { get; set; }


        [ProtoMember(7)]
        
        public decimal AgencyCommissionValue { get; set; }

        [ProtoMember(8)]
        public bool HasObjective { get; set; }
    }
}
