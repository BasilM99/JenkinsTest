using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories;

namespace Noqoush.AdFalcon.Domain.Common.Model.Account
{
    [DataContract()]
    public enum AccountFundTransStatusIds
    {
        [EnumMember]
        Committed = 0,
        [EnumMember]
        Pending = 1,
        [EnumMember]
        Failed = 2,
    }


}