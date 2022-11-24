using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API
{
    [DataContract]
    public class AppSiteStatisticsReport
    {
        [DataMember]
        public int Date { get; set; }

        [DataMember]
        public int? TimeId { get; set; }

        [DataMember]
        public string d { get; set; }

        [DataMember]
        public string aid { get; set; }

        [DataMember]
        public string an { get; set; }

        [DataMember]
        public long i { get; set; }

        [DataMember]
        public decimal rv { get; set; }

        [DataMember]
        public long r { get; set; }

        [DataMember]
        public long c { get; set; }
    }
}
