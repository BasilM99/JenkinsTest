using ArabyAds.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;
namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [ProtoContract]
    public class TransactionVATResultDto
    {

       [ProtoMember(1)]
        public string Name { get; set; }
       [ProtoMember(2)]
        public int AccountId { get; set; }
       [ProtoMember(3)]
        public decimal Amount { get; set; }

       [ProtoMember(4)]
        public decimal VATAmount { get; set; }

       [ProtoMember(5)]
        public DateTime TransactionDate { get; set; }
    }
}
