using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class AdvertiserAccountUserDto
    {
        [DataMember]

        public int ID { get; set; }
        [DataMember]

        public bool Read { get; set; }
        [DataMember]

        public bool Write { get; set; }
        [DataMember]

        public bool IsDeleted { get; set; }
        [DataMember]


        public UserDto User { get; set; }
        [DataMember]

        public AdvertiserAccountDto Link { get; set; }


    }




    [DataContract]
    public class AdvertiserAccountReadOnlyUserDto
    {
        [DataMember]

        public int ID { get; set; }
     
        [DataMember]

        public bool IsDeleted { get; set; }
        [DataMember]


        public UserDto User { get; set; }
        [DataMember]

        public AdvertiserAccountDto Link { get; set; }

        [DataMember]

        public InvitationDto Invitation { get; set; }

    }
}
