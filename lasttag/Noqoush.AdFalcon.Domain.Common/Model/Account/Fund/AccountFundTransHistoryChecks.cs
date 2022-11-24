using System;

namespace Noqoush.AdFalcon.Domain.Model.Account.Fund
{
    public class AccountFundTransHistoryCheck : AccountFundTransHistory
    {
        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }
        public virtual string CheckNo { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual bool IsCollected { get; set; }
        public virtual string IssuerName { get; set; }
        public virtual string IssuerBankName { get; set; }
        public virtual string IssuerBankBranch { get; set; }
        
    }
}
