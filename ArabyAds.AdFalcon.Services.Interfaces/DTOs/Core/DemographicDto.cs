using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class DemographicDto
    {
       [ProtoMember(1)]
        public GenderDto Gender { get; set; }

       [ProtoMember(2)]
        public AgeGroupDto AgeGroup { get; set; }
    }
}
