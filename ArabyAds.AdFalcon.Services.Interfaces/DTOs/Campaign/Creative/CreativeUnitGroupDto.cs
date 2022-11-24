using System;
using System.Collections.Generic;
using ProtoBuf;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
   
        [ProtoContract]
        public class CreativeUnitGroupDto : LookupDto
        {
       [ProtoMember(1)]
        public string Code { get; set; }

       [ProtoMember(2)]
        public virtual int MaxSize { get; set; }


    }
}
