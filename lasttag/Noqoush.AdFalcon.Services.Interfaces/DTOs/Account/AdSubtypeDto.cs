using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
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

    public class AdSubtypeDto: LookupDto
    {
        [DataMember]
        public  AdSubTypes Code { set; get; }
        [DataMember]
        public int AdTypeId { set; get; }
        [DataMember]
        public AdPermissionDto Permission { set; get; }
        [DataMember]
        public bool hide { get; set; }

        [DataMember]
        public IList<int> AdActionTypeIds { get; set; }

    }
}
