using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CampaignServerSettingDto
    {
        [DataMember]
        public virtual int ID { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        [Range(1, 2592000)]
        public int? AdRequestCacheLifeTime { get; set; }

        [DataMember]
        public virtual IList<CampaignFrequencyCappingDto> FrequencyCappingList { get; set; }

        [DataMember]
        public virtual IEnumerable<CampaignFrequencyCappingDto> DefultFrequencyCappingList { get; set; }
        [DataMember]
        public AgencyCommission AgencyCommission { get; set; }



        [DataMember]
        
        public decimal AgencyCommissionValue { get; set; }
    }
}
