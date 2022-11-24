using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class ChartDto
    {
       [ProtoMember(1)]
        public DateTime Xaxis { get; set; }
       [ProtoMember(2)]
        public decimal? Yaxis { get; set; }
       [ProtoMember(3)]

        public string XaxisString { get; set; }
    }
}
