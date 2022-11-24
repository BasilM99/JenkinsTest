using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class TrackingEventDto
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public int ValidFor { get; set; }
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public int? DefaultFrequencyCapping { get; set; }
        [DataMember]
        public string EventDescription { get; set; }

    }
}
