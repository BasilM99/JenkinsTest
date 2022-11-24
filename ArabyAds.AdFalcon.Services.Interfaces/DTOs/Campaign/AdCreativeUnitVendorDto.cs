using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [ProtoContract]
    public class AdCreativeUnitVendorDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public int UnitId { get; set; }
       [ProtoMember(3)]
        public int VendorId { get; set; }
       [ProtoMember(4)]
        public string VendorText { get; set; }
    }
}
