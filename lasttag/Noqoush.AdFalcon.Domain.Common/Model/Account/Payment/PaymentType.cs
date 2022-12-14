using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Core;


namespace Noqoush.AdFalcon.Domain.Common.Model.Account.Payment
{
    [DataContract()]
    public enum PaymentTypeIds
    {
        [EnumMember]
        Cash = 1,
        [EnumMember]
        WireTransfer = 2,
        [EnumMember]
        PayPal = 3,
        [EnumMember]
        Check = 4,
        [EnumMember]
        FundTransfer = 6,
        [EnumMember]
        OverBudgetReturn =8,
            
    }
 
}

