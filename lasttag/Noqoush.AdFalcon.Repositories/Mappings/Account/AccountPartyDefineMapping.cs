using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class AccountPartyDefineMapping : ClassMap<AccountPartyDefine>
    {
        public AccountPartyDefineMapping()
        {
            Table("`account_party_Define`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountPartyDefine'");
            References(x => x.Account, "AccountId").Nullable().Cascade.None().LazyLoad();
            References(x => x.Party, "PartyId").Not.Nullable().Cascade.None().LazyLoad();

            
        }
    }
}