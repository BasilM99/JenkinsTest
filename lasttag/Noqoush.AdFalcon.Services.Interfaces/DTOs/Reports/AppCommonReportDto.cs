using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.Framework.Utilities;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class AppCommonReportDto : BaseAppSiteResultDto
    {
       
        [DataMember]
        public int Date { get; set; }

        [DataMember]
        public int? TimeId { get; set; }

  
            [DataMember]
            public string DateRangeProp { get { return DateRange; } set { } }

   
    }
}
