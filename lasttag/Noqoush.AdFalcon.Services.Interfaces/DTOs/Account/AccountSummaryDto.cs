using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class AccountSummaryDto
    {
        [DataMember]
        public int AccountId
        {
            get;
            set;
        }

        [DataMember]
        public  decimal RoundedEarning
        {
            get;
            set;
        }

        [DataMember]
        public decimal RoundedFunds
        {
            get;
            set;
        }

        [DataMember]
        public decimal TotalPayments
        {
            get;
            set;
        }

    }
}
