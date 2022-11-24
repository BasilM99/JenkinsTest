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
    [ProtoInclude(100,typeof(AddExternalAudSegmentTargetingRequest))]
    public class SaveAudSegmentTargetingRequest : AudienceIdsMessages
    {
       
        [ProtoMember(1)]
        public List<AudienceSegmentDto> Segments { get; set; }
       
    }
    [ProtoContract]
    public class AddExternalAudSegmentTargetingRequest : SaveAudSegmentTargetingRequest
    {
       
        [ProtoMember(1)]
        public string Group { get; set; }
    }
}
