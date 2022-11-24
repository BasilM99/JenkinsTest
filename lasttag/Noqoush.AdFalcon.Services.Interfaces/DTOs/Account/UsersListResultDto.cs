﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class UsersListResultDto
    {
        [DataMember]
        public IEnumerable<UserDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }


    [DataContract]
    public class AccountDSPRequestListResultDto
    {
        [DataMember]
        public IEnumerable<AccountDSPRequestDto> Items { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}
