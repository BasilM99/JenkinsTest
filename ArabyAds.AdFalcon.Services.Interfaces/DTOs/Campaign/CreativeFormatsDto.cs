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
    public class CreativeFormatsDto : LookupDto
    {
       [ProtoMember(1)]
       // [Required(ResourceName = "Targeting_InvalidAdvertiserId", ResourceSet ="Error")]
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

       [ProtoMember(2)]
        public virtual string Code { set; get; }
       [ProtoMember(3)]
        public virtual string Description { set; get; }
    }

}
