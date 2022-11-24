using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Model.Account.Payment
{
    public class PaymentWire : Payment
    {
        public virtual BankAccountPaymentDetails AccountPaymentDetail { get; set; }
        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }
    }
}
