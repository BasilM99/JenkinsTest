using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [Serializable]
    [ProtoContract]
    public class CreativeVendorKeywordDto 
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public string Keyword { get; set; }
       [ProtoMember(3)]
        public int VendorId { get; set; }
    }
}
