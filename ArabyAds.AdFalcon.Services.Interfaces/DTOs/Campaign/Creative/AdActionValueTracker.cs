using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdActionValueTrackerDto
    {

       [ProtoMember(1)]
        public string URL { get; set; }

       [ProtoMember(2)]
        public string JS { get; set; }
    }
}
