using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{

    [ProtoContract]
    [ProtoInclude(100, typeof(SaveAudSegmentTargetingRequest))]
    public class AudienceIdsMessages
    {
        [ProtoMember(1)]
        public int AdgroupId { get; set; }
        [ProtoMember(2)]
        public int IdAccAdv { get; set; }
        [ProtoMember(3)]
        public int DpId { get; set; }

        public override string ToString()
        {
            return $"{AdgroupId}_{IdAccAdv}_{DpId}";
        }
    }
   
}
