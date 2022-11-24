using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
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

    public class AdSubtypeDto: LookupDto
    {
       [ProtoMember(1)]
        public  AdSubTypes Code { set; get; }
       [ProtoMember(2)]
        public int AdTypeId { set; get; }
       [ProtoMember(3)]
        public AdPermissionDto Permission { set; get; }
       [ProtoMember(4)]
        public bool hide { get; set; }

       [ProtoMember(5)]
        public IList<int> AdActionTypeIds { get; set; } = new List<int>();

    }
}
