using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class SaveAdRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public AdCreativeSaveDto AdCreative { get; set; }
    }
    [ProtoContract]
    public class SaveBidModifierRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public IList<AdGroupBidModifierDto> AdGroupBidModifiersDto { get; set; }
    }

    
}
