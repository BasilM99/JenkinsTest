using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [DataContract]
  public class KeyValueDto
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }

       // [DataMember]
        //public int? Order { get; set; }

    }
}
