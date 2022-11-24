using ArabyAds.AdFalcon.Services.Interfaces;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.Core
{
    [ProtoContract]
    public class metriceGroupColumnDto
    {
       [ProtoMember(1)]
        public metriceColumnDto metriceColumn { set; get; }
       [ProtoMember(2)]
        public metriceGroupDto metriceGroup { set; get; }
       [ProtoMember(3)]
        public string Deatils { set; get; }
    }
}
