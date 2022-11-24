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
    public class GetAdCreativeRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public int? AdCreativeId { get; set; }

        [ProtoMember(2)]
        public int? AdType { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{AdCreativeId?.ToString() ?? "Null"}_{AdType?.ToString() ?? "Null"}";
        }
    }
}
