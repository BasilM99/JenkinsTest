using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Model.Account.Payment
{
    public class PaymentPaypal : Payment
    {
        public virtual PayPalAccountPaymentDetails SystemPaymentDetail { get; set; }
        public virtual PayPalAccountPaymentDetails AccountPaymentDetail { get; set; }
    }
}
