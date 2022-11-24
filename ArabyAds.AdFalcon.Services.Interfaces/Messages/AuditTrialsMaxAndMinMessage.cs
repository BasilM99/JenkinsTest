using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class AuditTrialsMaxAndMinMessage
    {
        [ProtoMember(1)]
        public long Max { get; set; }
        [ProtoMember(2)]
        public long Min { get; set; }
    }
}
