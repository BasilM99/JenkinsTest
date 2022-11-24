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
    public class GetCampaignRequest
    {
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public CampaignType Type { get; set; }

        [ProtoMember(3)]
        public CampaignType Othertype { get; set; }

        public override string ToString()
        {
            return $"{CampaignId}_{Type}_{Othertype}";
        }


    }
}
