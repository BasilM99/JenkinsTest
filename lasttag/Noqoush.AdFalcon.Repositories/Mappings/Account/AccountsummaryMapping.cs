using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using FluentNHibernate;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class AccountSummaryMapping : ClassMap<AccountSummary>
    {
        public AccountSummaryMapping()
        {
            Table("`accountsummary`");

            OptimisticLock.Version();
            Map(x => x.Earning);
            Map(x => x.Credit);
            Map(x => x.Funds);
            Map(x => x.TotalPayments);

            //Id(Reveal.Member<AccountSummary>("AccountId"));//.Column("AccountId").GeneratedBy.Foreign("Account");
            //HasOne(Reveal.Member<AccountSummary, Domain.Model.Account.Account>("Account")).Constrained().ForeignKey("AccountId").Constrained();

            Id(Reveal.Member<AccountSummary>("AccountId")).Column("AccountId").GeneratedBy.Foreign("Account");
            HasOne(Reveal.Member<AccountSummary, Domain.Model.Account.Account>("Account")).Constrained();
        }
    }
}