using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]

    public class AdPermissionDto : LookupDto
    {
        [DataMember]
        public PortalPermissionsCode Code { get; set; }

    }
}
