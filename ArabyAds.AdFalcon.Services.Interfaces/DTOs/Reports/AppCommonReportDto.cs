using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.Framework.Utilities;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class AppCommonReportDto : BaseAppSiteResultDto
    {
       
       [ProtoMember(1)]
        public int Date { get; set; }

       [ProtoMember(2)]
        public int? TimeId { get; set; }

  
           [ProtoMember(3)]
            public string DateRangeProp { get { return DateRange; } set { } }

   
    }
}
