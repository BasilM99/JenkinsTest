using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
    public class KeywordTargetingDto : TargetingBaseDto
    {
        [DataMember]
        public KeywordDto Keyword { get; set; }
    }
}
