using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative
{
    [DataContract]
    public class AdCreativeUnitTrackerDto
    {
        [DataMember]
        [Noqoush.Framework.DataAnnotations.Required()]
        public string Url { get; set; }

        [DataMember]
        public int AdGroupEventId { get; set; }

        [DataMember]
        public string AdGroupEventName { get; set; }

        [DataMember]
        public int AdCreativeUnitId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public IList<AdActionValueTrackerDto> ImpressionURls { get; set; }


        [DataMember]
        //[Noqoush.Framework.DataAnnotations.Required()]
        public string JS { get; set; }


        [DataMember]
        //[Noqoush.Framework.DataAnnotations.Required()]
        public bool IsAllowedToSaveImpressionTracker { get; set; }
    }
}
