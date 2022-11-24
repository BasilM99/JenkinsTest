using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdActionValueTrackerDto
    {

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string JS { get; set; }
    }
}
