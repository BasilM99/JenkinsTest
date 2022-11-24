using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign
{


    [ProtoContract]
    public class AdvertiserDto : LookupDto
    {
       [ProtoMember(1)]
        [Required(ResourceName = "Targeting_InvalidAdvertiserId", ResourceSet ="Error")]
        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        //[DataMember]
        //public virtual int AdvertiserBusinessId { get; set; }
        [RegularExpression(@"^(https?:\/\/)?[a-zA-Z0-9]+(?:(?:\.|\-)[a-zA-Z0-9]+)+(?:\:\d+)?(?:\/[\w\-]+)*(?:\/?|\/\w+\.[a-zA-Z]{2,6}(?:\?[\w]+\=[\w\-]+)?)?(?:\&[\w]+\=[\w\-]+)*$", ResourceName = "UrlMsg")]
       [Required]
       [ProtoMember(2)]
        public virtual string DomainURL { get; set; }
       [ProtoMember(3)]
        public virtual string Description { get; set; }

    }

    [ProtoContract]
    public class AdvertiserResult {
       [ProtoMember(1)]
        public IEnumerable<AdvertiserDto> Items { get; set; } = new List<AdvertiserDto>();
        [ProtoMember(2)]
        public int TotalCount { get; set; }
    }

}
