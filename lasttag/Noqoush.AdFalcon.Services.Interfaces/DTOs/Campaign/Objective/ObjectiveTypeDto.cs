using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    [DataContract]
    public class ObjectiveTypeDto : LookupDto
    {
        [DataMember]
        public virtual IEnumerable<AdActionTypeDto> AdActionTypes { get; set; }
    }
}
