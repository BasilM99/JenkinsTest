using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
   

    [ProtoContract]
    public class ClickTagTrackerDto
    {
       [ProtoMember(1)]
        public int ID { get; set; }
       [ProtoMember(2)]
        public string VariableName { get; set; }
       [ProtoMember(3)]
        public string TrackingUrl { get; set; }
       [ProtoMember(4)]
        public bool IsDeleted { get; set; }
  
    }

}
