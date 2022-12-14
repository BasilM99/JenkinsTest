using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core
{
    [DataContract()]
    public enum SystemPaymentDetailTypes
    {
        [EnumMember]
        CreditCard = 1,
        [EnumMember]
        Bank = 2,
        [EnumMember]
        PayPal = 3
    }
 
}
