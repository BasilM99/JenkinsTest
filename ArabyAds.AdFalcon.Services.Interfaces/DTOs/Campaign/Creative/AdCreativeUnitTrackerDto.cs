using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [ProtoContract]
    public class AdCreativeUnitTrackerDto
    {
       [ProtoMember(1)]
        [ArabyAds.Framework.DataAnnotations.Required()]
        public string Url { get; set; }

       [ProtoMember(2)]
        public int AdGroupEventId { get; set; }

       [ProtoMember(3)]
        public string AdGroupEventName { get; set; }

       [ProtoMember(4)]
        public int AdCreativeUnitId { get; set; }

       [ProtoMember(5)]
        public bool IsDeleted { get; set; }

       [ProtoMember(6)]
        public IList<AdActionValueTrackerDto> ImpressionURls { get; set; } = new List<AdActionValueTrackerDto>();


        [ProtoMember(7)]
        //[ArabyAds.Framework.DataAnnotations.Required()]
        public string JS { get; set; }


       [ProtoMember(8)]
        //[ArabyAds.Framework.DataAnnotations.Required()]
        public bool IsAllowedToSaveImpressionTracker { get; set; }
    }
}
