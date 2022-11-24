using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class CostModelWrapperDto : LookupDto
    {
       [ProtoMember(1)]
        public int CostModel { get; set; }

       [ProtoMember(2)]
        public int Factor { get; set; }
        

       [ProtoMember(3)]
        public AppScope Scope { get; set; }
    }
}
