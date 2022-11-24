using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.Messages.Requests.Campaign;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    [ProtoInclude(100, typeof(RenameGroupRequest))]
    [ProtoInclude(101, typeof(CloneAdgroupRequest))]
    [ProtoInclude(102, typeof(QueryAdsBidRequest))]
    [ProtoInclude(103, typeof(CampaignIdAdgroupIdAdIdMessage))]
    [ProtoInclude(104, typeof(GetAdGroupDynamicBiddingConfigRequest))]
    [ProtoInclude(105, typeof(GetAdGroupCostElementRequest))]
    [ProtoInclude(106, typeof(GetCostModelWrapperTrackingEventsRequest))]
    [ProtoInclude(107, typeof(DeleteTrackingEventRequest))]
    [ProtoInclude(108, typeof(IsDeleteTrackingEventAllowedRequest))]
    [ProtoInclude(109, typeof(GetAdCreativeRequest))]
    [ProtoInclude(110, typeof(SaveAdRequest))]
    [ProtoInclude(111, typeof(IsDeleteTrackingEventAllowedRequest))]
    [ProtoInclude(112, typeof(CampaignIdAdgroupIdAdIdsMessage))]
    [ProtoInclude(113, typeof(SaveBidModifierRequest))]
    [ProtoInclude(114, typeof(SetAdsBidRequest))]
    [ProtoInclude(115, typeof(GetTargetingRequest))]

    public class CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public int AdgroupId { get; set; }

        public override string ToString()
        {
            return $"{CampaignId}_{AdgroupId}";
        }

    }
}
