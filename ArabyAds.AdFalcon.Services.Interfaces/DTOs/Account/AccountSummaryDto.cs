using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class AccountSummaryDto
    {
       [ProtoMember(1)]
        public int AccountId
        {
            get;
            set;
        }

       [ProtoMember(2)]
        public  decimal RoundedEarning
        {
            get;
            set;
        }

       [ProtoMember(3)]
        public decimal RoundedFunds
        {
            get;
            set;
        }

       [ProtoMember(4)]
        public decimal TotalPayments
        {
            get;
            set;
        }

    }
}
