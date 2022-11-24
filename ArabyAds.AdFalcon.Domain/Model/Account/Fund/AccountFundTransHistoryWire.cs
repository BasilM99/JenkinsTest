namespace ArabyAds.AdFalcon.Domain.Model.Account.Fund
{
    public class AccountFundTransHistoryWire : AccountFundTransHistory
    {
        public virtual BankAccountPaymentDetails AccountPaymentDetail { get; set; }
        public virtual BankAccountPaymentDetails SystemPaymentDetail { get; set; }
    }
}
