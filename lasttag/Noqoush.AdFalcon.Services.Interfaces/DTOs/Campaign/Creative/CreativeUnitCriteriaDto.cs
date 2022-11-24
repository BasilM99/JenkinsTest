using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class CreativeUnitCriteriaDto
    {
        [DataMember]
        public int? AdTypeId { get; set; }

        [DataMember]
        public int? CreativeUnitId { get; set; }

        [DataMember]
        public int DeviceTypeId { get; set; }

        [DataMember]
        public string Group { get; set; }

    }
}
