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
    public class FormatAdCreativeContentRequest
    {
        [ProtoMember(1)]
        public string Content { get; set; }
        [ProtoMember(2)]
        public int CreativeId { get; set; }

        public override string ToString()
        {
            return $"{Content ?? "Null"}_{CreativeId}";
        }
    }
}
