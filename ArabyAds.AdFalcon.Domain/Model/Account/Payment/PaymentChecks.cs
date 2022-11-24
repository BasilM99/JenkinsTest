using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Model.Account.Payment
{
    public class PaymentCheck : Payment
    {
        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }
        public virtual string CheckNo { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual bool IsCollected { get; set; }
        public virtual string BeneficiaryName { get; set; }
        
    }
}
