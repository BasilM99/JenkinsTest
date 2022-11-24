using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class ChartDto
    {
        [DataMember]
        public DateTime Xaxis { get; set; }
        [DataMember]
        public object Yaxis { get; set; }
        [DataMember]

        public string XaxisString { get; set; }
    }
}
