using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class MetricDto : LookupDto
    {
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string MetricTarget { get; set; }

        [DataMember]
        public string Color { get; set; }
    }
}
