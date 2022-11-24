using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetTrialSessionRequest
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public bool IsAdminApp { get; set; }

        public override string ToString()
        {
            return $"{Id}_{IsAdminApp}";
        }
    }
}
