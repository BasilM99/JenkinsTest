using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class AccountAPIAccessDto
    {
        [DataMember]
        public string APISecretKey { get; set; }

        [DataMember]
        public string APIClientId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public bool AllowAPIAccess { get; set; }
    }
}
