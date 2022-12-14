using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class BankaccountMapping : ClassMap<BankAccountPaymentDetails>
    {
        public BankaccountMapping()
        {
            Table("`bankaccount`");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'BankAccount'");

            // Id(Reveal.Member<BankAccount>("AccountId")).Column("AccountId").GeneratedBy.Foreign("Account");

            //HasOne(Reveal.Member<BankAccount, Domain.Model.Account.Account>("Account")).Constrained().ForeignKey("AccountId").Constrained();
            // HasOne(p => p.).PropertyRef(p => p).Cascade.All();

            References(x => x.Account, "AccountId");
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