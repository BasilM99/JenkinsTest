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
    public class CheckAppsitesCostModelCompatableWitCampaignRequest
    {
        [ProtoMember(1)]
        public int CampaignId { get; set; }
        [ProtoMember(2)]
        public List<int> Appsites { get; set; }
        [ProtoMember(3)]
        public int? AdGroupId { get; set; }
        [ProtoMember(4)]
        public int? GroupCostModelWrapperID { get; set; }
        [ProtoMember(5)]
        public bool CheckExisting { get; set; }

        public override string ToString()
        {
            return $"{CampaignId}_{Appsites?.ToString() ?? "Null"}_{AdGroupId?.ToString() ?? "Null"}_{GroupCostModelWrapperID?.ToString() ?? "Null"}_{CheckExisting}";
        }
    }
}
