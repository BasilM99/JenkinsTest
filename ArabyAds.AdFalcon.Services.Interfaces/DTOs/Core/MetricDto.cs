using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class MetricDto : LookupDto
    {
       [ProtoMember(1)]
        public int Id { get; set; }


       [ProtoMember(2)]
        public string Code { get; set; }

       [ProtoMember(3)]
        public string MetricTarget { get; set; }

       [ProtoMember(4)]
        public string Color { get; set; }
    }


    [ProtoContract]
    public class MetricResultDto : LookupDto
    {
        [ProtoMember(1)]
        public int MetricId { get; set; }


        [ProtoMember(2)]
        public string Code { get; set; }

        [ProtoMember(3)]
        public string MetricTarget { get; set; }

        [ProtoMember(4)]
        public string Color { get; set; }
    }
}
