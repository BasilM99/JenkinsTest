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
    public class CloneAdgroupRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{Name ?? "Null"}";
        }
    }
}
