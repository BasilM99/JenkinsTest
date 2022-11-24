using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AddDefaultAdGroupTrackingEventByIdRequest
    {
        [ProtoMember(1)]
        public int AdGroupId { get; set; }
        [ProtoMember(2)]
        public int CostModelWrapperId { get; set; }
        [ProtoMember(3)]
        public int? OldCostModelWrapper { get; set; }
    }
}
