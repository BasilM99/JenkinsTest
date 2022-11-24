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
    [ProtoInclude(100,typeof(CloneAdRequest))]
    public class CampaignIdAdgroupIdAdIdMessage : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public int AdId { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{AdId}";
        }
    }
}
