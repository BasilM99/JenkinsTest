using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]

    public class AdPermissionDto : LookupDto
    {
       [ProtoMember(1)]
        public PortalPermissionsCode Code { get; set; }

    }
}
