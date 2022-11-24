using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{


    [DataContract]
    public class AdvertiserDto : LookupDto
    {
        [DataMember]
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
        [DataMember]
        public virtual string DomainURL { get; set; }
        [DataMember]
        public virtual string Description { get; set; }

    }

    [DataContract]
    public class AdvertiserResult {
        [DataMember]
        public IEnumerable<AdvertiserDto> Items { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
    }

}
