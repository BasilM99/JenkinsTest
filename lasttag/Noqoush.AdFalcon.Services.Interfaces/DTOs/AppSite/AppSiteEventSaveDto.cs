using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteEventSaveDto
    {
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public decimal? MinBid { get; set; }

        [DataMember]
        public bool IsBillable { get; set; }
    }
}
