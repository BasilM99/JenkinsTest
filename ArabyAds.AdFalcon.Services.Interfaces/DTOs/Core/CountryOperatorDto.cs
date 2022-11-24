using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class CountryOperatorDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }
       [ProtoMember(2)]
        public LocalizedStringDto Name { get; set; }
       [ProtoMember(3)]
        public List<OperatorDto> Operators { get; set; }
    }
}
