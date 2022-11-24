using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Web.Controllers.Model.User
{
    public class Receipt
    {
        public decimal VATAmount { get; set; }
        public decimal Amount { get; set; }

        public decimal TotoalAmount { get {
                return VATAmount+Amount;

            } }
        public string NoqoushReceiptNumber { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public string TransactionDate { get; set; }
    }
}
