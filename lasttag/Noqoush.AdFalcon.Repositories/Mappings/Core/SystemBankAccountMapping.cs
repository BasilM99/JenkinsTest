using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class SystemBankAccountMapping : ClassMap<SystemBankAccount>
    {
        public SystemBankAccountMapping()
        {
            Table("`system_bank_account`");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'SystemBankAccount'");
            Map(x => x.BankAddress);
            Map(x => x.BankName);
            Map(x => x.BeneficiaryName);
            Map(x => x.IsActive);
            Map(x => x.ActiveFrom);
            Map(x => x.ActiveTo).Nullable();
            Map(x => x.RecipientAccountNumber);
            Map(x => x.SWIFT, "SWIFT");

        }
    }
}