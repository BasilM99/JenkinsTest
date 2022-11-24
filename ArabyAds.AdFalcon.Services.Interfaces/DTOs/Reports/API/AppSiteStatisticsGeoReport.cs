using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.API
{
    [ProtoContract]
    public class AppSiteStatisticsGeoReport : AppSiteStatisticsReport
    {
       [ProtoMember(1)]
        public string cc { get; set; } 
    }
}
