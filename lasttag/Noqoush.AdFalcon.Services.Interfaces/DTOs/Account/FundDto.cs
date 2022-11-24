using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Account
{
    [DataContract]
    public class FundResultDto
    {
        [DataMember]
        public IEnumerable<FundTransactionDto> Items { get; set; }


        

        [DataMember]
        public int Total { get; set; }
    }

    //[DataContract]
    //public  class FundDto
    //{
    //    [DataMember]
    //    public DateTime FundDate { get; set; }

    //    [DataMember]
    //    public string TransactionId { get; set; }

    //    [DataMember]
    //    public decimal Amount { get; set; }

    //    [DataMember]
    //    public string FundType { get; set; }

    //    [DataMember]
    //    public string Currency { get; set; }

    //    [DataMember]
    //    public string Payee { get; set; }

    //}
}
