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
    public class AdvertiserAccountDto
    {
        [DataMember]

        public int Id { get; set; }

        [DataMember]

        public AdvertiserDto Advertiser { get; set; }
        [DataMember]

        public int UserId { get; set; }
        [DataMember]

        public int AccountId { get; set; }

        [DataMember]

        public bool IsRestricted { get; set; }

        

        [DataMember]
        public string Name { get; set; }

    }

}
