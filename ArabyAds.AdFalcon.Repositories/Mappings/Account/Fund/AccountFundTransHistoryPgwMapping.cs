using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Noqoush.AdFalcon.Repositories.Mappings.Account
{
    public class AccountFundTransHistoryPgwMapping : SubclassMap<AccountFundTransHistoryPgw>
    {
        public AccountFundTransHistoryPgwMapping()
        {
            Table("`account_fund_trans_history_pgw`");
            KeyColumn("id");
            Map(x => x.ErrorCode, "error_code");
            Map(x => x.ExtraInfo, "extra_info");
            Map(x => x.PgwStatus, "pgw_status");
            Map(x => x.PgwTransactionId, "pgw_transaction_id");
            Map(x => x.ResponseDate, "response_date");
            Map(x => x.AccountFundPgwId, "account_fund_pgw_id");
            Map(x => x.ReceiptNumber, "receipt_number");
        }
    }
}