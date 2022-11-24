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
    public class AdvertiserAccountDto
    {
       [ProtoMember(1)]

        public int Id { get; set; }

       [ProtoMember(2)]

        public AdvertiserDto Advertiser { get; set; }
       [ProtoMember(3)]

        public int UserId { get; set; }
       [ProtoMember(4)]

        public int AccountId { get; set; }

       [ProtoMember(5)]

        public bool IsRestricted { get; set; }

        

       [ProtoMember(6)]
        public string Name { get; set; }

    }

}
