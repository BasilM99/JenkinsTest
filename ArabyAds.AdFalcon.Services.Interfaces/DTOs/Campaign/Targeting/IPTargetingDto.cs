using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
    public class IPTargetingDto : TargetingBaseDto
    {
       [ProtoMember(1)]
        //[RegularExpression(@"^(?:(?:25[0-5]2[0-4][0-9][01]?[0-9][0-9]?)\.){3}(?:25[0-5]2[0-4][0-9][01]?[0-9][0-9]?)$")]
        [RegularExpression(@"\b(?:\d{1,3}\.){3}\d{1,3}\b")]
        //[RegularExpression(@"[0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]")]
        public string StartRange { get; set; }
       [ProtoMember(2)]
        [RegularExpression(@"^(?:(?:25[0-5]2[0-4][0-9][01]?[0-9][0-9]?)\.){3}(?:25[0-5]2[0-4][0-9][01]?[0-9][0-9]?)$")]
        public string EndRange { get; set; }
       [ProtoMember(3)]
        public string Description { get; set; }
    }
}
