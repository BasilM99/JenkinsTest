using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdTypeDto : LookupDto
    {
        [DataMember]
        public AdPermissionDto AdPermission { get; set; }
        [DataMember]
        public IList<AdSubtypeDto> Subtypes { get; set; }
        [DataMember]
        public bool hide { get; set; }
    }
}
