using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [DataContract]
    public class AppSiteEventDto
    {
        [DataMember]
        public virtual int ID { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public virtual string EventName { get; set; }
        [DataMember]
        public virtual bool IsBillable { get; set; }
        [DataMember]
        public virtual decimal? MinBid { get; set; }

        public virtual string MinBidValue
        {
            get
            {
                return MinBid.HasValue ? MinBid.Value.ToString("F3") : null;
            }
        }
    }
}
