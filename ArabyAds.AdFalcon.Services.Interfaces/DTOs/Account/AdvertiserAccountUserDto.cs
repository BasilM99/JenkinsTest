using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;

using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class AdvertiserAccountUserDto
    {
       [ProtoMember(1)]

        public int ID { get; set; }
       [ProtoMember(2)]

        public bool Read { get; set; }
       [ProtoMember(3)]

        public bool Write { get; set; }
       [ProtoMember(4)]

        public bool IsDeleted { get; set; }
       [ProtoMember(5)]


        public UserDto User { get; set; }
       [ProtoMember(6)]

        public AdvertiserAccountDto Link { get; set; }


    }




    [ProtoContract]
    public class AdvertiserAccountReadOnlyUserDto
    {
       [ProtoMember(1)]

        public int ID { get; set; }
     
       [ProtoMember(2)]

        public bool IsDeleted { get; set; }
       [ProtoMember(3)]


        public UserDto User { get; set; }
       [ProtoMember(4)]

        public AdvertiserAccountDto Link { get; set; }

       [ProtoMember(5)]

        public InvitationDto Invitation { get; set; }

    }
}
