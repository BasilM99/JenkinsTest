using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class CreativeVendorKeywordDto 
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Keyword { get; set; }
        [DataMember]
        public int VendorId { get; set; }
    }
}
