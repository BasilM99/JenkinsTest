using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Repositories;

using ArabyAds.AdFalcon.Domain.Common.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Account
{
    [DataContract()]
    public enum AccountFundTransTypeIds
    {
        [EnumMember]
        CreditCard = 1,
        [EnumMember]
        WireTransfer = 2,
        [EnumMember]
        Cash = 3,
        [EnumMember]
        BalanceTransfer = 4,
        [EnumMember]
        BankCheck = 5,
        [EnumMember]
        FreeCredit = 6,
        [EnumMember]
        PayPal = 7,
        [EnumMember]
        FundTransfer = 8,
        [EnumMember]
        OverBudgetRefund = 9
    }

  
      
}