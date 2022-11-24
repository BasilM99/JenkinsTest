using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Account.Payment
{
    public class PaymentWire : Payment
    {
        public virtual BankAccountPaymentDetails AccountPaymentDetail { get; set; }
        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }
    }
}
