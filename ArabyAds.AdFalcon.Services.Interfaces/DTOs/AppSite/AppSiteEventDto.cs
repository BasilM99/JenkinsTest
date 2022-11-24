using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteEventDto
    {
       [ProtoMember(1)]
        public virtual int ID { get; set; }
       [ProtoMember(2)]
        public int EventId { get; set; }
       [ProtoMember(3)]
        public virtual string EventName { get; set; }
       [ProtoMember(4)]
        public virtual bool IsBillable { get; set; }
       [ProtoMember(5)]
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
