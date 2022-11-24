using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetRootObjectNameRequest
    {
        [ProtoMember(1)]
        public int RootObjectTypeID { get; set; }
        [ProtoMember(2)]
        public int RootObjectId { get; set; }

        [ProtoMember(3)]
        public string ObjectTypeName { get; set; }

        public override string ToString()
        {
            return $"{RootObjectTypeID}_{RootObjectId}_{ObjectTypeName ?? "Null"}";
        }
    }
}
