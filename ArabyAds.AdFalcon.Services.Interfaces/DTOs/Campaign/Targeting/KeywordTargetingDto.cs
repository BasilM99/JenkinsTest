using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class KeywordTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        public KeywordDto Keyword { get; set; }
    }
}
