using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class BaseCriteriaDto
    {
        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }

        [DataMember]
        public DateTime RFromDate { get; set; }

        [DataMember]
        public DateTime RToDate { get; set; }
        [DataMember]
        public int AdvertiserId { get; set; }

        [DataMember]
        public int AccountAdvertiserId { get; set; }
    }
}
