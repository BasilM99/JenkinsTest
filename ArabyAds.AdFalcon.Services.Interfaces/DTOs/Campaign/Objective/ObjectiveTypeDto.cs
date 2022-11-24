using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    [ProtoContract]
    public class ObjectiveTypeDto : LookupDto
    {
       [ProtoMember(1)]
        public virtual IEnumerable<AdActionTypeDto> AdActionTypes { get; set; } = new List<AdActionTypeDto>();
    }
}
