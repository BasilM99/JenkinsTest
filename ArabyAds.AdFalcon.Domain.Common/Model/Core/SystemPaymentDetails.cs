using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Core
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
