using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Persistence.Mappings;


namespace Noqoush.AdFalcon.Repositories.Mappings.Account
{
    public class AccountFundTransTypeMapping : ClassMap<AccountFundTransType>
    {
        public AccountFundTransTypeMapping()
        {
            Table("`account_fund_trans_type`");
            Id(x => x.ID, "id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountFundTransType'");
            Map(x => x.AllowImpersonate, "AllowImpersonate");
            Map(x => x.IsDeleted, "IsDeleted");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }

    public class AccountFundTypeMapping : ClassMap<AccountFundType>
    {
        public AccountFundTypeMapping()
        {
            Table("`account_fund_type`");
            Id(x => x.ID, "id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountFundType'");
            Map(x => x.Multiplier, "Multiplier");
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}