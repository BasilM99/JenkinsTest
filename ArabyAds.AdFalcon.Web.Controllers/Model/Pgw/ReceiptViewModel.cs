using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Model.Pgw
{
    public class ReceiptViewModel
    {
        public string Title { get; set; }
        public string TransactionDate { get; set; }
        public string ReceiptNo { get; set; }
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string VATAmount { get; set; }
        public string Message { get; set; }
    }
}
