using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class UserAccountsMapping : ClassMap<UserAccounts>
    {
        public UserAccountsMapping()
        {
            Table("user_accounts");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                               MappingSettings._nextHi,
                                               MappingSettings._maxLo,
                                               "TableKey = 'UserAccounts'");
            References(x => x.Account, "AccountId");
            References(x => x.User, "UserId");
            // Map(x => x.IsSecondPrimaryUser, "IsSecondPrimaryUser");
            Map(x => x.UserType, "UserType").CustomType<UserType>(); 
        }
    }
}