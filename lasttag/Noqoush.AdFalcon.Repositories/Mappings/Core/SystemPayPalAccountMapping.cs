using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class SystemPayPalAccountMapping : ClassMap<SystemPayPalAccount>
    {
        public SystemPayPalAccountMapping()
        {
            Table("`system_paypal_account`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'SystemPayPalAccount'");
            Map(x => x.UserName);
            Map(x => x.IsActive);
            Map(x => x.ActiveFrom);
            Map(x => x.ActiveTo).Nullable();
        }
    }
}