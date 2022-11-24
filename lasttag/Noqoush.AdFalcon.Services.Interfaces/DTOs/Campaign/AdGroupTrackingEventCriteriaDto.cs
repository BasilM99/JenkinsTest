using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdGroupTrackingEventCriteriaDto
    {
        [DataMember]
        public int CampaignId { get; set; }

        [DataMember]
        public int AdGroupId { get; set; }

        [DataMember]
        public int CostModelWrapperId { get; set; }

        [DataMember]
        public bool LoadDetaultTrackingEvents { get; set; }

    }
}
