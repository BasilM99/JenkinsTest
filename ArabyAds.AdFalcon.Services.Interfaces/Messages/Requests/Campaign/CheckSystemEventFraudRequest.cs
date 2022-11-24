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
    public class CheckSystemEventFraudRequest
    {
        [ProtoMember(1)]
        public string Code { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Code ?? "Null"}_{Name ?? "Null"}";
        }
    }
}
