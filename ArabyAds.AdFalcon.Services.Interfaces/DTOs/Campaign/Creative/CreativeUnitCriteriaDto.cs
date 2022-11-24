using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class CreativeUnitCriteriaDto
    {
       [ProtoMember(1)]
        public int? AdTypeId { get; set; }

       [ProtoMember(2)]
        public int? CreativeUnitId { get; set; }

       [ProtoMember(3)]
        public int DeviceTypeId { get; set; }

       [ProtoMember(4)]
        public string Group { get; set; }

    }
}
