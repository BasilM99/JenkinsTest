using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    [ProtoContract]
    public class FundResultDto
    {
        [ProtoMember(1)]
        public IEnumerable<FundTransactionDto> Items { get; set; } = new List<FundTransactionDto>();


        

       [ProtoMember(2)]
        public int Total { get; set; }
    }

    //[ProtoContract]
    //public  class FundDto
    //{
    //   [ProtoMember()]
    //    public DateTime FundDate { get; set; }

    //   [ProtoMember()]
    //    public string TransactionId { get; set; }

    //   [ProtoMember()]
    //    public decimal Amount { get; set; }

    //   [ProtoMember()]
    //    public string FundType { get; set; }

    //   [ProtoMember()]
    //    public string Currency { get; set; }

    //   [ProtoMember()]
    //    public string Payee { get; set; }

    //}
}
