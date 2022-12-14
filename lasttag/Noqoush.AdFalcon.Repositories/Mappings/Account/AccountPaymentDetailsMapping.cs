using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class AccountPaymentDetailsMapping : ClassMap<AccountPaymentDetails>
    {
        public AccountPaymentDetailsMapping()
        {
            Table("`account_payment_details`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountPaymentDetails'");
            References(x => x.Account, "AccountId").Nullable();
            Map(x => x.IsDefault);
            Map(x => x.IsSystem);
            Map(x => x.AccountType, "typeId").CustomType(typeof(PayemntAccountType));
            Map(x => x.SubType, "subtypeId").CustomType(typeof(PayemntAccountSubType));
            Map(x => x.IsActive);
            Map(x => x.ActiveFrom);
            Map(x => x.ActiveTo).Nullable();

        }
    }

    public class BankAccountPaymentDetailMapping : SubclassMap<BankAccountPaymentDetails>
    {
        public BankAccountPaymentDetailMapping()
        {
            Table("`bank_account_payment_details`");
            KeyColumn("Id");
            Map(x => x.BankAddress);
            Map(x => x.BankName);
            Map(x => x.BeneficiaryName);
            Map(x => x.RecipientAccountNumber);
            Map(x => x.SWIFT, "SWIFT");
        }
    }

    public class PayPalAccountPaymentDetailsMapping : SubclassMap<PayPalAccountPaymentDetails>
    {
        public PayPalAccountPaymentDetailsMapping()
        {
            Table("`paypal_account_payment_details`");
            KeyColumn("Id");
            Map(x => x.UserName);
            Map(x => x.IsPrimary);
        }
    }
}