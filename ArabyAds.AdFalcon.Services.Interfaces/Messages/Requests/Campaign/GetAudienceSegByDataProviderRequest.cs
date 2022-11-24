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
    public class GetAudienceSegByDataProviderRequest
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string q { get; set; }
        [ProtoMember(3)]
        public string cultures { get; set; }

        public override string ToString()
        {
            return $"{Id}_{q ?? "Null"}_{cultures ?? "Null"}";
        }


    }
}
