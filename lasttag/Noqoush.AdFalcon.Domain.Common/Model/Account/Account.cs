

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;


namespace Noqoush.AdFalcon.Domain.Common.Model.Account
{
    [DataContract(Name = "AddPaymentResponse")]
    public enum AddPaymentResponse
    {
        [EnumMember]
        // [EnumText("0", "WeekDays")]
        Done = 1,
        [EnumMember]
        //[EnumText("0", "WeekDays")]
        NoChanges = 2,
        [EnumMember]
        //[EnumText("0", "WeekDays")]
        ChangeDefault = 3,
        //  [EnumText("0", "WeekDays")]
        [EnumMember]
        Reset = 4
    }
    [Flags]
    [DataContract(Name = "AccountRole")]
    public enum AccountRole
    {
        [EnumMember]
       
        AnonymousUser = 0,

        [EnumMember]
        [EnumText("NormalUser", "AccountRole")]
        NormalUser = 1,
        [EnumMember]
        Advertiser = 2,
        [EnumMember]
        Publisher = 4,
        [EnumMember]
        [EnumText("DSP", "AccountRole")]
        DSP = 8,
        [EnumMember]
        AppOps = 16,
        [EnumMember]
        AdOps = 32,
        [EnumMember]
        Admin = 64,
        [EnumMember]
        FinanceManager = 128,
        [EnumMember]
        [EnumText("DataProvUser", "AccountRole")]
        DataProvider = 256,

        AdOpsAppOps = AdOps | AppOps,
        AdvertiserDSP = Advertiser | DSP,
        All = Admin | AdOps | AppOps | DSP | Publisher | Advertiser | DataProvider | NormalUser
    }

    /*
    public class Account : Party//IEntity<int>
    {
        private IAccountDSPRequestRepository _AccountDSPRequestRepository = IoC.Instance.Resolve<IAccountDSPRequestRepository>();
        private IPortalPermisionRepository _PortalPermisionRepository = IoC.Instance.Resolve<IPortalPermisionRepository>();
        private IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository = IoC.Instance.Resolve<IAccountPortalPermissionsRepository>();
        private IConfigurationManager _configurationManager = IoC.Instance.Resolve<IConfigurationManager>();
        private ISecurityService _securityService = IoC.Instance.Resolve<ISecurityService>();
        private IAccountRepository _accountRepository = IoC.Instance.Resolve<IAccountRepository>();
        private static ICountryVATRepository _CountryVATRepository = IoC.Instance.Resolve<ICountryVATRepository>();
        private static IUserAccountsRepository _userAccountsRepository = IoC.Instance.Resolve<IUserAccountsRepository>();

        private IList<AccountPaymentDetails> _PaymentDetails;

        public virtual IList<AccountPaymentDetails> PaymentDetails
        {
            get
            {
                return _PaymentDetails;
            }

            set
            {
                _PaymentDetails = value;
            }
        }
        public virtual IList<AccountDiscount> Discounts { get; set; }
        public virtual float? DefaultRevenuePercentage { get; set; }
        public virtual AgencyCommission? AgencyCommission { get; set; }

        public virtual decimal AgencyCommissionValue { get; set; }
        public virtual AccountRole AccountRole { get; set; }
        public virtual void setAgencyCommission(AgencyCommission agC)
        {
            if (agC == Noqoush.AdFalcon.Domain.Common.Model.Account.AgencyCommission.Undefined)
            {
                this.AgencyCommission = null;
            }
            else
            {
                this.AgencyCommission = agC;
            }

        }

        public virtual AgencyCommission getAgencyCommission()
        {
            if (this.AgencyCommission.HasValue)
                return this.AgencyCommission.Value;
            else
                return Noqoush.AdFalcon.Domain.Common.Model.Account.AgencyCommission.Undefined;

        }
        public virtual string GetDefaultRevenuePercentageDescriper(string value)
        {


            if (!string.IsNullOrEmpty(value))
            {
                return (Convert.ToDecimal(value) * 100).ToString("F2") + "%";

            }
            return string.Empty;
        }


        public virtual decimal Fund
        {
            get; set;
        }
        public virtual decimal Earning
        {
            get; set;
        }
        public virtual string TaxNo { get; set; }
        public virtual Document TaxRegistration { get; set; }

        public virtual User PrimaryUser { get; set; }
        public virtual AccountSummary AccountSummary { get; set; }
        public virtual string UserAgreementVersion { get; set; }
        public virtual IList<HouseAd> HouseAds { get; set; }
        public virtual AccountAPIAccess APIAccess { get; set; }
        public virtual bool AllowAPIAccess { get; set; }
        public virtual int AccountBusinessId { get; set; }
        // public virtual Account Parent { get; set; }

        public virtual Buyer buyer { get; set; }

        public virtual void SetPrimaryUser(User user)
        {
            throw new System.NotImplementedException();
        }
        public virtual void SetBankAccount(BankAccountPaymentDetails bankAccountPaymentDetails)
        {
            throw new System.NotImplementedException();
        }
        public virtual string GetName()
        {
            return this.PrimaryUser.GetName();
        }
        public virtual string GetAccountName()
        {
            return this.PrimaryUser.GetAccountName();
        }
        public virtual void RemoveDiscount()
        {
            if ((Discounts != null) && (Discounts.Count > 0))
            {
                var cuurentdata = Framework.Utilities.Environment.GetServerTime().AddSeconds(1);
                var currentDiscount = Discounts.FirstOrDefault(
                    x =>
                    x.Discount.FromDate <= cuurentdata &&
                    (!x.Discount.ToDate.HasValue || x.Discount.ToDate >= cuurentdata));

                // reactive current discount 
                if (currentDiscount != null)
                    currentDiscount.DeActive();
            }
        }
        public virtual void AddDiscount(Core.Discount discount)
        {
            if ((Discounts == null) || (Discounts.Count == 0))
            {
                Discounts = new List<AccountDiscount>();
            }

            var cuurentdata = Framework.Utilities.Environment.GetServerTime().AddSeconds(1);
            var currentDiscount = Discounts.FirstOrDefault(
                    x =>
                    x.Discount.FromDate <= cuurentdata &&
                    (!x.Discount.ToDate.HasValue || x.Discount.ToDate >= cuurentdata));

            if ((currentDiscount != null) && (currentDiscount.Discount.Value.Equals(discount.Value)))
                return;

            // reactive current discount 
            if (currentDiscount != null)
                currentDiscount.DeActive();
            // insert new discount
            // update discount info
            discount.FromDate = cuurentdata;
            Discounts.Add(new AccountDiscount { Account = this, Discount = discount });
        }
        public virtual void AddFund(decimal fundAmount)
        {
            AccountSummary.AddToTotalFunds(fundAmount);
            this.Fund = this.AccountSummary.Funds;
            this.Earning = this.AccountSummary.Earning;
        }
        public virtual Payment.Payment AddPayment(Payment.Payment payment)
        {
            if (this.AccountSummary.RoundedEarning < payment.Amount)
            {
                //create business Exception to hold error data list 
                var error = new BusinessException();
                error.Errors.Add(new ErrorData { ID = "EarningLessThanPaymentBR" });
                throw error;
            }
            payment.Account = this;
            payment.CreationDate = Framework.Utilities.Environment.GetServerTime();
            this.AccountSummary.TotalPayments += payment.Amount;
            this.AccountSummary.Earning -= payment.Amount;
            this.Fund = this.AccountSummary.Funds;
            this.Earning = this.AccountSummary.Earning;
            // PublishAccountAmountForKafka(this.AccountSummary.Funds, this.AccountSummary.Earning);
            return payment;
        }

        public virtual void ResetIsPrimaryPayPalAccounts()
        {
            var payPalDetailInfo = PaymentDetails.Where(x => x.AccountType == PayemntAccountType.PayPal);

            foreach (var payPal in payPalDetailInfo)
            {
                var paypalItem = (payPal as PayPalAccountPaymentDetails);

                if (paypalItem.IsPrimary)
                {
                    (payPal as PayPalAccountPaymentDetails).ConvertToNormal();
                }
            }
        }

        public virtual AddPaymentResponse ResetAccountPaymentDetails(PayemntAccountType accountType)
        {
            var result = AddPaymentResponse.NoChanges;
            List<AccountPaymentDetails> paymentDetailInfo = null;

            if (accountType == PayemntAccountType.PayPal)
            {
                paymentDetailInfo = PaymentDetails.Where(x => x.AccountType == accountType && x.IsActive && (x as PayPalAccountPaymentDetails).IsPrimary).ToList();
            }
            else
            {
                paymentDetailInfo = PaymentDetails.Where(x => x.AccountType == accountType && x.IsActive).ToList();
            }

            foreach (var accountPaymentDetail in paymentDetailInfo)
            {
                accountPaymentDetail.DeActive();
                result = AddPaymentResponse.Reset;
            }
            return result;
        }
        public virtual AddPaymentResponse AddAccountPaymentDetail(AccountPaymentDetails accountPaymentDetails, bool isDefault)
        {
            if (PaymentDetails == null)
            {
                PaymentDetails = new List<AccountPaymentDetails>();
            }

            AccountPaymentDetails paymentDetailinfo = null;

            if (accountPaymentDetails.AccountType == PayemntAccountType.PayPal)
            {
                paymentDetailinfo = PaymentDetails.FirstOrDefault(x => x.AccountType == accountPaymentDetails.AccountType && x.IsActive && x.SubType == accountPaymentDetails.SubType && (x as PayPalAccountPaymentDetails).IsPrimary);
            }
            else
            {
                paymentDetailinfo = PaymentDetails.FirstOrDefault(x => x.AccountType == accountPaymentDetails.AccountType && x.IsActive && x.SubType == accountPaymentDetails.SubType);
            }

            if ((accountPaymentDetails.Equals(paymentDetailinfo)) && (paymentDetailinfo == null || (paymentDetailinfo.IsDefault == isDefault)))
                return AddPaymentResponse.NoChanges;

            if (accountPaymentDetails.AccountType == PayemntAccountType.PayPal) { (accountPaymentDetails as PayPalAccountPaymentDetails).ConvertToPrimary(); }

            if (paymentDetailinfo != null)
            {
                if (accountPaymentDetails.AccountType == PayemntAccountType.PayPal) { (paymentDetailinfo as PayPalAccountPaymentDetails).ConvertToNormal(); }

                if ((paymentDetailinfo.IsDefault != isDefault))
                {
                    paymentDetailinfo.IsDefault = isDefault;
                    //return AddPaymentResponse.ChangeDefault;
                }
                //else
                {
                    //update old payment info
                    paymentDetailinfo.DeActive();
                }
            }
            //set new payment info
            accountPaymentDetails.ActiveFrom = Framework.Utilities.Environment.GetServerTime();
            accountPaymentDetails.Account = this;
            accountPaymentDetails.IsActive = true;
            accountPaymentDetails.ActiveTo = null;
            accountPaymentDetails.IsDefault = isDefault;
            PaymentDetails.Add(accountPaymentDetails);
            return AddPaymentResponse.Done;
        }
        public virtual AccountPaymentDetails GetPayPalAccount(PayPalAccountPaymentDetails accountPaymentDetails)
        {
            if (PaymentDetails == null)
            {
                PaymentDetails = new List<AccountPaymentDetails>();
            }
            return PaymentDetails.FirstOrDefault(x => x is PayPalAccountPaymentDetails &&
                                                      x.AccountType == accountPaymentDetails.AccountType &&
                                                      x.IsActive && x.SubType == accountPaymentDetails.SubType &&
                                                      (x as PayPalAccountPaymentDetails).UserName.Trim().ToLower() ==
                                                      accountPaymentDetails.UserName.Trim().ToLower());

        }
        public virtual AccountPaymentDetails GetBankAccount(BankAccountPaymentDetails accountPaymentDetails)
        {
            if (PaymentDetails == null)
            {
                PaymentDetails = new List<AccountPaymentDetails>();
            }
            var items = PaymentDetails.Where(x => x is BankAccountPaymentDetails &&
                                                  x.AccountType == accountPaymentDetails.AccountType &&
                                                  x.IsActive && x.SubType == accountPaymentDetails.SubType);
            return items.FirstOrDefault(item => item.Equals(accountPaymentDetails));
        }
        public virtual AddPaymentResponse AddAccountPaymentDetailSystem(AccountPaymentDetails accountPaymentDetails)
        {
            if (PaymentDetails == null)
            {
                PaymentDetails = new List<AccountPaymentDetails>();
            }

            //set new payment info
            accountPaymentDetails.ActiveFrom = Framework.Utilities.Environment.GetServerTime();
            accountPaymentDetails.Account = this;
            accountPaymentDetails.IsActive = true;
            accountPaymentDetails.ActiveTo = null;
            accountPaymentDetails.IsDefault = false;
            PaymentDetails.Add(accountPaymentDetails);
            return AddPaymentResponse.Done;
        }
        public override string GetDescription()
        {
            return GetName() + string.Format("-  {0}: {1}  ", ResourceManager.Instance.GetResource("email", "Global"), PrimaryUser.EmailAddress) + (string.IsNullOrWhiteSpace(PrimaryUser.Company) ? string.Empty : string.Format("-  {0}: {1}", ResourceManager.Instance.GetResource("Company", "Global"), PrimaryUser.Company));
        }

        public virtual Core.Discount GetActiveDiscount()
        {
            Core.Discount discount = null;
            var time = Framework.Utilities.Environment.GetServerTime();
            if (this.Discounts!=null&& this.Discounts.Count>0)
            {
                var accountDiscount = this.Discounts.FirstOrDefault(x => time >= x.Discount.FromDate && (!x.Discount.ToDate.HasValue || time <= x.Discount.ToDate));
                if (accountDiscount != null)
                {
                    discount = new Core.Discount { FromDate= accountDiscount.Discount.FromDate, Value = accountDiscount.Discount.Value, Type = DiscountType.Percentage };//, ToDate =(Discount.ToDate };
                }
            }
            return discount;
        }
        public virtual void GoDSP(bool activate = true)
        {

            var accountDSP = _AccountDSPRequestRepository.GetByEmailAddress(PrimaryUser.EmailAddress);
            if (accountDSP != null)
            {
                accountDSP.Account = this;

                if (string.IsNullOrWhiteSpace(accountDSP.Company))
                {
                    this.Name = accountDSP.FirstName + " " + accountDSP.LastName;
                }
                else
                {

                    this.Name = accountDSP.Company;
                }
                _AccountDSPRequestRepository.Save(accountDSP);
            }
            bool IsAudiPer = _AccountPortalPermissionsRepository.Query(x => x.Account.ID == ID && x.Permission.Code == PortalPermissionsCode.Audience).Count() == 0;
            if (IsAudiPer)
            {
                var audiPer = _PortalPermisionRepository.GetByCode(PortalPermissionsCode.Audience);

                AccountPortalPermissions audiAdPer = new AccountPortalPermissions();
                audiAdPer.Account = this;
                audiAdPer.Permission = audiPer;
                _AccountPortalPermissionsRepository.Save(audiAdPer);
            }

            bool IsInvPer = _AccountPortalPermissionsRepository.Query(x => x.Account.ID == ID && x.Permission.Code == PortalPermissionsCode.InventorySource).Count() == 0;
            if (IsInvPer)
            {
                var InvPer = _PortalPermisionRepository.GetByCode(PortalPermissionsCode.InventorySource);

                AccountPortalPermissions InvAdPer = new AccountPortalPermissions();

                InvAdPer.Account = this;
                InvAdPer.Permission = InvPer;
                _AccountPortalPermissionsRepository.Save(InvAdPer);
            }

            bool IsDealPer = _AccountPortalPermissionsRepository.Query(x => x.Account.ID == ID && x.Permission.Code == PortalPermissionsCode.PMPDeal).Count() == 0;
            if (IsInvPer)
            {
                var DealPer = _PortalPermisionRepository.GetByCode(PortalPermissionsCode.PMPDeal);

                AccountPortalPermissions DealAdPer = new AccountPortalPermissions();

                DealAdPer.Account = this;
                DealAdPer.Permission = DealPer;
                _AccountPortalPermissionsRepository.Save(DealAdPer);
            }

            UserAgreementVersion = _configurationManager.GetConfigurationSetting(null, null, "CompareDSPUserAgreementVersion");
            AccountRole = AccountRole.DSP;
            this.UserAgreementVersion = UserAgreementVersion;

            if (activate)
            {
                _securityService.ActivateUser(this.PrimaryUser.EmailAddress);

                this.PrimaryUser.Activate();
                this.PrimaryUser.Status = new UserStatus();

                this.PrimaryUser.Status.SetActiveStatus();
            }
            _accountRepository.Save(this);

        }

        public virtual void GoDataProvider()
        {

            this.AccountRole = AccountRole.DataProvider;
            _accountRepository.Save(this);

            bool IsAudiPer = _AccountPortalPermissionsRepository.Query(x => x.Account.ID == ID && x.Permission.Code == PortalPermissionsCode.Audience).Count() == 0;
            if (IsAudiPer)
            {
                var audiPer = _PortalPermisionRepository.GetByCode(PortalPermissionsCode.Audience);

                AccountPortalPermissions audiAdPer = new AccountPortalPermissions();
                audiAdPer.Account = this;
                audiAdPer.Permission = audiPer;
                _AccountPortalPermissionsRepository.Save(audiAdPer);
            }

        }
        public virtual string GetTaxRegistrationExpression()
        {

            if (this.PrimaryUser.Country != null)
                return _CountryVATRepository.GetTaxRegistrationExpressionByCountryID(this.PrimaryUser.Country.ID);


            return string.Empty;

        }
        public virtual decimal GetVATValue()
        {

            if (this.PrimaryUser.Country != null)
                return _CountryVATRepository.GetVATValueByCountryID(this.PrimaryUser.Country.ID);


            return 0;

        }

        public virtual Account Clone()
        {
            var AccountOb = new Account();
            AccountOb.Name = this.Name;
            AccountOb.TypeNameString = this.TypeNameString;
          //AccountOb.PaymentDetails=this.PaymentDetails;
            //AccountOb.Discounts = this.Discounts;
            AccountOb.DefaultRevenuePercentage = this.DefaultRevenuePercentage;
            AccountOb.AgencyCommission = this.AgencyCommission;
            AccountOb.TaxNo = this.TaxNo;
            AccountOb.TaxRegistration = this.TaxRegistration;
            AccountOb.AgencyCommissionValue = this.AgencyCommissionValue;

            AccountOb.PrimaryUser = this.PrimaryUser;
           // AccountOb.AccountSummary = this.AccountSummary;

            AccountOb.buyer = this.buyer;
            AccountOb.AccountBusinessId = this.AccountBusinessId;
           // AccountOb.AllowAPIAccess = this.AllowAPIAccess;
           // AccountOb.APIAccess = this.APIAccess;
            //AccountOb.HouseAds = this.HouseAds;
            AccountOb.UserAgreementVersion = this.UserAgreementVersion;
      
            return AccountOb;
        }


       
    }*/
}

