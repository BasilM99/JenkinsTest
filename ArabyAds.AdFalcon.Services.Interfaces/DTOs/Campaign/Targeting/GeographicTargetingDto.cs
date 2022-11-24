using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class GeographicTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public LocationDto  Location { get; set; }
    }
}
