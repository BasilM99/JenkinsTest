using System;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace ArabyAds.AdFalcon.Repositories.Mappings.Account
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