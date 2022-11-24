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
    public class CreativeFormatsDto : LookupDto
    {
        [DataMember]
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

        [DataMember]
        public virtual string Code { set; get; }
        [DataMember]
        public virtual string Description { set; get; }
    }

}
