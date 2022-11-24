using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.API
{
    [DataContract]
    public class AppSiteStatisticsGeoReport : AppSiteStatisticsReport
    {
        [DataMember]
        public string cc { get; set; } 
    }
}
