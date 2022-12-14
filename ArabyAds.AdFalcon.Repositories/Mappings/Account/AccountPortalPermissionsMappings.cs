using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class AccountPortalPermissionsMappings : ClassMap<AccountPortalPermissions>
    {
        public AccountPortalPermissionsMappings()
        {
            Table("account_permissions");
            Id(x => x.ID).GeneratedBy.Identity();
            References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.Permission, "PermissionId");
        }
    }
}