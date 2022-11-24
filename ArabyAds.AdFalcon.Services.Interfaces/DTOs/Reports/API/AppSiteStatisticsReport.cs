using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API
{
    [ProtoContract]
    [ProtoInclude(100,typeof(AppSiteStatisticsGeoReport))]
    public class AppSiteStatisticsReport
    {
       [ProtoMember(1)]
        public int Date { get; set; }

       [ProtoMember(2)]
        public int? TimeId { get; set; }

       [ProtoMember(3)]
        public string d { get; set; }

       [ProtoMember(4)]
        public string aid { get; set; }

       [ProtoMember(5)]
        public string an { get; set; }

       [ProtoMember(6)]
        public long i { get; set; }

       [ProtoMember(7)]
        public decimal rv { get; set; }

       [ProtoMember(8)]
        public long r { get; set; }

       [ProtoMember(9)]
        public long c { get; set; }
    }
}
