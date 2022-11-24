using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports.Dashboard
{
    [ProtoContract]
  public class KeyValueDto
    {
       [ProtoMember(1)]
        public string Key { get; set; }

       [ProtoMember(2)]
        public string Value { get; set; }

       //[ProtoMember()]
        //public int? Order { get; set; }

    }
}
