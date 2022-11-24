using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class AccountDSPReqestResultDto
    {
       [ProtoMember(1)]
        public string RequestCode { get; set; }
       [ProtoMember(2)]
        public bool IsAlreadyRegistered { get; set; }
       [ProtoMember(3)]
        public bool Success { get; set; }

       [ProtoMember(4)]
        public int accountId { get; set; }
    }
}
