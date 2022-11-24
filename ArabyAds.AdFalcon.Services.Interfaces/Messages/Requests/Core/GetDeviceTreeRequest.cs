using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetDeviceTreeRequest
    {
        [ProtoMember(1)]
        public int PlatformId { set; get; }
        [ProtoMember(2)]
        public int DeviceConstraint { set; get; }

        public override string ToString()
        {
            return $"{PlatformId}_{DeviceConstraint}";
        }
    }
}
