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
    public class QueryAdsBidRequest : CampaignIdAdgroupIdMessage
    {
        [ProtoMember(1)]
        public decimal Bid { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}_{Bid}";
        }
    }
}
