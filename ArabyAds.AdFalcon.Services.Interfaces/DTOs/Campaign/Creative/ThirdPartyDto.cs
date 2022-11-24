using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
 
[ProtoContract]

public class ThirdPartyTrackerDto
{
       [ProtoMember(1)]
        public int ID { get; set; }


       [ProtoMember(2)]
        public string VendorID { get; set; }
       [ProtoMember(3)]
        public string ScriptURL { get; set; }




   [ProtoMember(4)]
    public string ExecutionErrorTrackerURL { get; set; }
   [ProtoMember(5)]
    public string ParametersURL { get; set; }

       [ProtoMember(6)]
        public bool IsNotChanged { get; set; }
        




   [ProtoMember(7)]
        public bool IsDeleted { get; set; }

    }

}
