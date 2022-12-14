using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
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