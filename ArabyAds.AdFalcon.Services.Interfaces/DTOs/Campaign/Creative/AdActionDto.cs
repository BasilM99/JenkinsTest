using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdTypeDto : LookupDto
    {
       [ProtoMember(1)]
        public AdPermissionDto AdPermission { get; set; }
       [ProtoMember(2)]
        public IList<AdSubtypeDto> Subtypes { get; set; } = new List<AdSubtypeDto>();
        [ProtoMember(3)]
        public bool hide { get; set; }
    }
}
