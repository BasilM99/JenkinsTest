using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
   

    [DataContract]
    public class ClickTagTrackerDto
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string VariableName { get; set; }
        [DataMember]
        public string TrackingUrl { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
  
    }

}
