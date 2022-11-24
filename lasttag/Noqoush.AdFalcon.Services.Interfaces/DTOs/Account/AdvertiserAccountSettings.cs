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
    public class AdvertiserAccountSettings
    {
        [DataMember]
        public List<AdvertiserAccountUserDto> Assignments { get; set; }
        [DataMember]
        public int AccountAdvertiserId { get; set; }

        [DataMember]
        public AgencyCommission AgencyCommission { get; set; }

        [DataMember]
        public decimal AgencyCommissionValue { get; set; }
        [DataMember]
        public bool IsRestricted { get; set; }


    }


    [DataContract]
    public class AdvertiserAccountSettingsForReadOnly
    {
        [DataMember]
        public List<AdvertiserAccountReadOnlyUserDto> Assignments { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int InvitationId { get; set; }
        [DataMember]
        public List<int> LinkIds { get; set; }

        [DataMember]
        public UserType UserType { get; set; }


    }

}
