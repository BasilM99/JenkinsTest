using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class GetTrialDetailsSessionRequest
    {

        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public bool IsAdminApp { get; set; }
        [ProtoMember(3)]
        [DefaultValue(true)]
        public bool CollectInfo { get; set; } = true;

        public override string ToString()
        {
            return $"{Id}_{IsAdminApp}_{CollectInfo}";
        }

    }
}
