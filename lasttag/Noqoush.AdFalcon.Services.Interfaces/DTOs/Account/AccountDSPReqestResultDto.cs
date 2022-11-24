using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class AccountDSPReqestResultDto
    {
        [DataMember]
        public string RequestCode { get; set; }
        [DataMember]
        public bool IsAlreadyRegistered { get; set; }
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int accountId { get; set; }
    }
}
