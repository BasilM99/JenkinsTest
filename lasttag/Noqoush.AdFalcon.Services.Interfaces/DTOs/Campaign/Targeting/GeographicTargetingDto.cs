using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class GeographicTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public LocationDto  Location { get; set; }
    }
}
