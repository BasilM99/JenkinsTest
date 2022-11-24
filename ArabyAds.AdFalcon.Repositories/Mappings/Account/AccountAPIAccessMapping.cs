using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class AccountAPIAccessMapping : ClassMap<AccountAPIAccess>
    {
        public AccountAPIAccessMapping()
        {
            Table("account_apiaccess");
            OptimisticLock.Version();
            Map(p => p.APIClientId);
            Map(p => p.APISecretKey);
            Id(Reveal.Member<AccountAPIAccess>("AccountId")).Column("AccountId").GeneratedBy.Foreign("Account");
            HasOne(Reveal.Member<AccountAPIAccess, Domain.Model.Account.Account>("Account")).Constrained().ForeignKey("AccountId").Constrained();
        }
    }
}
