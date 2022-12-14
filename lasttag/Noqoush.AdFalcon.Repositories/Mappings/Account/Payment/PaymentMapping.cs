using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account.Payment
{
    public class PaymentMapping : ClassMap<Domain.Model.Account.Payment.Payment>
    {
        public PaymentMapping()
        {
            Table("`account_payment_trans_history`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Payment'");
            Map(x => x.Amount);
            Map(x => x.VATAmount);
            Map(x => x.TransactionDate);
            Map(x => x.ForMonth);
            Map(x => x.TransactionId);
            Map(x => x.Comment);
            Map(x => x.AdFalconReceiptNo);
            Map(x => x.CreationDate);
            Map(x => x.IsDeleted);
            References(x => x.Currency, "CurrencyId");
            Map(x => x.ExchangeRate);
            Map(x => x.OriginalAmount);
            References(x => x.Attachment)
               .Column("AttachmentId")
               .Fetch.Select();
            References(x => x.Account)
                .Class(typeof(Domain.Model.Account.Account))
                .Not.Nullable()
                .Column("AccountId")
                .Fetch.Select();

            References(x => x.Type)
                .Class(typeof(PaymentType))
                .Not.Nullable()
                .Column("account_trans_type_Id")
                .Fetch.Select();
            References(x => x.User)
                .Class(typeof(User))
                .Not.Nullable()
                .Column("UserId")
                .Fetch.Select();
        }
    }

    public class PaymentWireMapping : SubclassMap<Domain.Model.Account.Payment.PaymentWire>
    {
        public PaymentWireMapping()
        {
            Table("`account_payment_trans_history_wire`");
            KeyColumn("Id");
            References(x => x.AccountPaymentDetail, "account_payment_detail_id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id");
        }
    }
    public class PaymentCheckMapping : SubclassMap<Domain.Model.Account.Payment.PaymentCheck>
    {
        public PaymentCheckMapping()
        {
            Table("`account_payment_trans_history_bchecks`");
            KeyColumn("Id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id");
            Map(x => x.CheckNo, "Check_no");
            Map(x => x.DueDate, "due_date");
            Map(x => x.IsCollected, "Collected");
            Map(x => x.BeneficiaryName);
        }
    }

    public class PaymentPaypalMapping : SubclassMap<Domain.Model.Account.Payment.PaymentPaypal>
    {
        public PaymentPaypalMapping()
        {
            Table("`account_payment_trans_history_paypal`");
            KeyColumn("Id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id");
            References(x => x.AccountPaymentDetail, "account_payment_detail_id");
        }
    }
   /* public class PaymentCashMapping : SubclassMap<Domain.Model.Account.Payment.PaymentCash>
    {
        public PaymentCashMapping()
        {
           
        }
    }*/
}