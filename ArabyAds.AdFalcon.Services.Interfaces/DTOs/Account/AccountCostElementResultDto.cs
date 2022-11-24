using System.Collections.Generic;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{

    [ProtoContract]
    public class AccountCostElementDto
    {
       [ProtoMember(1)]
        public decimal CostValue { get; set; }

       [ProtoMember(2)]
        public string Value { get; set; }
       [ProtoMember(3)]
        public bool Enabled { get; set; }
       [ProtoMember(4)]
        public int CostElmentId { get; set; }

       [ProtoMember(5)]
        public int? ID { get; set; }
       [ProtoMember(6)]
        public int AccountId { get; set; }

       [ProtoMember(7)]
        public int partyId { get; set; }
       [ProtoMember(8)]
        public int DataProviderId { get; set; }

       [ProtoMember(9)]
        public string DataProviderName { get; set; }
       [ProtoMember(10)]
        public string partyName { get; set; }

       
    }
    [ProtoContract]
    public class AccountCostElementResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<AccountCostElementDto> Items { get; set; } = new List<AccountCostElementDto>();
    
       [ProtoMember(2)]
        public int TotalCount { get; set; }
    }




    [ProtoContract]
    public class AccountFeeDto
    {
       [ProtoMember(1)]
        public decimal CostValue { get; set; }

       [ProtoMember(2)]
        public string Value { get; set; }
       [ProtoMember(3)]
        public bool Enabled { get; set; }
       [ProtoMember(4)]
        public int FeeId { get; set; }

       [ProtoMember(5)]
        public int? ID { get; set; }
       [ProtoMember(6)]
        public int AccountId { get; set; }
       [ProtoMember(7)]
        public int DataProviderId { get; set; }

       [ProtoMember(8)]
        public int partyId { get; set; }

       [ProtoMember(9)]
        public string partyName { get; set; }
    }
    [ProtoContract]
    public class AccountFeeResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<AccountFeeDto> Items { get; set; } = new List<AccountFeeDto>();

       [ProtoMember(2)]
        public int TotalCount { get; set; }
    }
}
