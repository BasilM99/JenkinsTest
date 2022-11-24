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
    public class CampaignIdAdgroupIdAdIdsMessage : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public int[] AdIds { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{AdIds?.ToString() ?? "Null"}";
        }
    }
}
