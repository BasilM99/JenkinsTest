using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdGroupTrackingEventCriteriaDto
    {
       [ProtoMember(1)]
        public int CampaignId { get; set; }

       [ProtoMember(2)]
        public int AdGroupId { get; set; }

       [ProtoMember(3)]
        public int CostModelWrapperId { get; set; }

       [ProtoMember(4)]
        public bool LoadDetaultTrackingEvents { get; set; }

    }
}
