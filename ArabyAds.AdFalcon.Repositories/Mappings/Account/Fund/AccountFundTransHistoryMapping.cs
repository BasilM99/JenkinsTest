using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Account.Fund;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.Fund
{
    public class AccountFundTransHistoryMapping : ClassMap<AccountFundTransHistory>
    {
        public AccountFundTransHistoryMapping()
        {
            Table("`account_fund_trans_history`");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountFundTransHistory'");
            Map(x => x.AccountId, "AccountId");
            Map(x => x.Amount, "Amount");
            Map(x => x.VATAmount, "VATAmount");
            
            Map(x => x.OriginalAmount);
            Map(x => x.CreatedById, "UserId");
            Map(x => x.CreationDate, "CreationDate");
            References(x => x.Currency, "CurrencyId");
            //Map(x => x.Payee, "Payee");
            Map(x => x.TransactionDate, "TransactionDate");
            Map(x => x.TransactionId);
            Map(x => x.ObjectRelatedId).Nullable();
            
            Map(x => x.NoqoushReceiptNumber, "ReceiptNumber");
            References(x => x.FundTransStatus, "StatusId");
            References(x => x.AccountFundType, "account_fund_type_Id");

            Map(x => x.Comment);
            References(x => x.FundTransType, "account_trans_type_id");
            References(x => x.Attachment)
               .Column("AttachmentId")
               .Fetch.Select();
        }
    }

    public class AccountFundTransHistoryPgwMapping : SubclassMap<AccountFundTransHistoryPgw>
    {
        public AccountFundTransHistoryPgwMapping()
        {
            Table("`account_fund_trans_history_pgw`");
            KeyColumn("id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id");
            Map(x => x.ErrorCode, "error_code");
            Map(x => x.ExtraInfo, "extra_info");
            Map(x => x.PgwStatus, "pgw_status");
            Map(x => x.ResponseDate, "response_date");
            Map(x => x.AccountFundPgwId, "account_fund_pgw_id");
            Map(x => x.ReceiptNumber, "receipt_number");
        }
    }

    public class AccountFundTransHistoryWireMapping : SubclassMap<AccountFundTransHistoryWire>
    {
        public AccountFundTransHistoryWireMapping()
        {
            Table("`account_fund_trans_history_wire`");
            KeyColumn("Id");
            References(x => x.AccountPaymentDetail, "account_payment_detail_id").Cascade.SaveUpdate();
            References(x => x.SystemPaymentDetail, "system_payment_detail_id").Cascade.SaveUpdate();
        }
    }
    public class AccountFundTransHistoryCheckMapping : SubclassMap<AccountFundTransHistoryCheck>
    {
        public AccountFundTransHistoryCheckMapping()
        {
            Table("`account_fund_trans_history_bchecks`");
            KeyColumn("Id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id");
            Map(x => x.CheckNo, "Check_no");
            Map(x => x.DueDate, "due_date");
            Map(x => x.IsCollected, "Collected");
            Map(x => x.IssuerName, "issuer_name");
            Map(x => x.IssuerBankName, "issuer_bank_name");
            Map(x => x.IssuerBankBranch, "issuer_bank_Branch");

        }
    }

    public class AccountFundTransHistoryPaypalMapping : SubclassMap<AccountFundTransHistoryPaypal>
    {
        public AccountFundTransHistoryPaypalMapping()
        {
            Table("`account_fund_trans_history_paypal`");
            KeyColumn("Id");
            References(x => x.SystemPaymentDetail, "system_payment_detail_id").Cascade.SaveUpdate(); 
            References(x => x.AccountPaymentDetail, "account_payment_detail_id").Cascade.SaveUpdate(); 

        }
    }
}