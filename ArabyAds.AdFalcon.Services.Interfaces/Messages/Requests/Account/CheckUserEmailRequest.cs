using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.Messages
{
    [ProtoContract]
    public class CheckUserEmailRequest
    {
        [ProtoMember(1)]
        public string EmailAddress { get; set; }
        [ProtoMember(2)]
        public bool CheckPendingEmail { get; set; }

        public override string ToString()
        {
            return $"{EmailAddress ?? "Null"}_{CheckPendingEmail}";
        }
    }
}
