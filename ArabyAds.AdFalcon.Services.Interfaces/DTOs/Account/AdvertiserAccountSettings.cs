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
    public class AdvertiserAccountSettings
    {
        [ProtoMember(1)]
        public List<AdvertiserAccountUserDto> Assignments { get; set; } = new List<AdvertiserAccountUserDto>();
       [ProtoMember(2)]
        public int AccountAdvertiserId { get; set; }

       [ProtoMember(3)]
        public AgencyCommission AgencyCommission { get; set; }

       [ProtoMember(4)]
        public decimal AgencyCommissionValue { get; set; }
       [ProtoMember(5)]
        public bool IsRestricted { get; set; }


    }


    [ProtoContract]
    public class AdvertiserAccountSettingsForReadOnly
    {
        [ProtoMember(1)]
        public List<AdvertiserAccountReadOnlyUserDto> Assignments { get; set; } = new List<AdvertiserAccountReadOnlyUserDto>();
       [ProtoMember(2)]
        public int UserId { get; set; }
       [ProtoMember(3)]
        public int InvitationId { get; set; }
       [ProtoMember(4)]
        public List<int> LinkIds { get; set; }

       [ProtoMember(5)]
        public UserType UserType { get; set; }


    }

}
