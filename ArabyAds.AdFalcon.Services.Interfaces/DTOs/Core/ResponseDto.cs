using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]

    public class ResponseDto
    {
       [ProtoMember(1)]
        public string Massage { get; set; }
       [ProtoMember(2)]
        public bool success { get; set; }

    }
}
