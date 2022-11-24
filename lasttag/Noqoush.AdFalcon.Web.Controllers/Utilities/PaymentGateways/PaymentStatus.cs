using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Web.Controllers.Utilities.PaymentGateways
{
    public class PaymentStatus
    {
        public int TransationID { get; set; }

        public bool IsCompleted { get; set; }

        public decimal Amount { get; set; }

        public string Message { get; set; }
    }
}
