namespace ArabyAds.AdFalcon.Domain.Model.Account.Fund
{
    public class AccountFundTransHistoryPaypal : AccountFundTransHistory
    {
        public virtual PayPalAccountPaymentDetails SystemPaymentDetail { get; set; }
        public virtual PayPalAccountPaymentDetails AccountPaymentDetail { get; set; }
       
    }
}
