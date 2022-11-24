using Noqoush.AdFalcon.Services.Interfaces;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.Core
{
    [DataContract]
    public class metriceGroupColumnDto
    {
        [DataMember]
        public metriceColumnDto metriceColumn { set; get; }
        [DataMember]
        public metriceGroupDto metriceGroup { set; get; }
        [DataMember]
        public string Deatils { set; get; }
    }
}
