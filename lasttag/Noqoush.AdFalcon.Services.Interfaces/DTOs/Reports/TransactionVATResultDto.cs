using Noqoush.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract]
    public class TransactionVATResultDto
    {

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal VATAmount { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }
    }
}
