using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.AppSite
{
    [ProtoContract]
    public class AppSiteEventSaveDto
    {
       [ProtoMember(1)]
        public int EventId { get; set; }

       [ProtoMember(2)]
        public decimal? MinBid { get; set; }

       [ProtoMember(3)]
        public bool IsBillable { get; set; }
    }
}
