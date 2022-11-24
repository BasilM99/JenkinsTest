using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class UsersListResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<UserDto> Items { get; set; } = new List<UserDto>();
       [ProtoMember(2)]
        public long TotalCount { get; set; }
    }


    [ProtoContract]
    public class AccountDSPRequestListResultDto
    {
       [ProtoMember(1)]
        public IEnumerable<AccountDSPRequestDto> Items { get; set; } = new List<AccountDSPRequestDto>();
        [ProtoMember(2)]
        public long TotalCount { get; set; }
    }
}
