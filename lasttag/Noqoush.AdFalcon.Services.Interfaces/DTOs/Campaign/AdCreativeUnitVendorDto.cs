using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class AdCreativeUnitVendorDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UnitId { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public string VendorText { get; set; }
    }
}
