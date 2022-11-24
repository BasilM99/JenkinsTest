using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class TrackingEventDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }

       [ProtoMember(2)]
        public string EventName { get; set; }

       [ProtoMember(3)]
        public int ValidFor { get; set; }
       [ProtoMember(4)]
        public string Code { get; set; }

       [ProtoMember(5)]
        public int? DefaultFrequencyCapping { get; set; }
       [ProtoMember(6)]
        public string EventDescription { get; set; }

    }
}
