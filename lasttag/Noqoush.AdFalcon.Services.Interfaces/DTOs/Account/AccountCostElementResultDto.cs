using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{

    [DataContract]
    public class AccountCostElementDto
    {
        [DataMember]
        public decimal CostValue { get; set; }

        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public int CostElmentId { get; set; }

        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int partyId { get; set; }
        [DataMember]
        public int DataProviderId { get; set; }

        [DataMember]
        public string DataProviderName { get; set; }
        [DataMember]
        public string partyName { get; set; }

       
    }
    [DataContract]
    public class AccountCostElementResultDto
    {
        [DataMember]
        public IEnumerable<AccountCostElementDto> Items { get; set; }
    
        [DataMember]
        public int TotalCount { get; set; }
    }




    [DataContract]
    public class AccountFeeDto
    {
        [DataMember]
        public decimal CostValue { get; set; }

        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public int FeeId { get; set; }

        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int DataProviderId { get; set; }

        [DataMember]
        public int partyId { get; set; }

        [DataMember]
        public string partyName { get; set; }
    }
    [DataContract]
    public class AccountFeeResultDto
    {
        [DataMember]
        public IEnumerable<AccountFeeDto> Items { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}
