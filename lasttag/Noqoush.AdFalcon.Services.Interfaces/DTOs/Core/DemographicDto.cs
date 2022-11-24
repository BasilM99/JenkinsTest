using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class DemographicDto
    {
        [DataMember]
        public GenderDto Gender { get; set; }

        [DataMember]
        public AgeGroupDto AgeGroup { get; set; }
    }
}
