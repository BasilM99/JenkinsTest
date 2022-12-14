using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Noqoush.AdFalcon.Repositories.Mappings.Account
{
    public class AccountFundTransStatusMapping : ClassMap<AccountFundTransStatus>
    {
        public AccountFundTransStatusMapping()
        {
            Table("`account_fund_trans_status`");
            Id(x => x.ID, "id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountFundTransStatus'");
            References(x => x.Name, "name_id");
        }
    }
}