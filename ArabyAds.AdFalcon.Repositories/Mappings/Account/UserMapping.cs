using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Tenant;
using ArabyAds.AdFalcon.Persistence.Mappings.Tenant;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("`users`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'User'");
            OptimisticLock.Version();
            Map(x => x.ActivationCode);
            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.City);
            Map(x => x.Company);
            Map(x => x.EmailAddress);
            Map(x => x.FirstName);
            Map(x => x.IsAllowNotifications);
            Map(x => x.LastName);
            Map(x => x.Password);
            References(x => x.Tenant, "TenantId").Not.Update();
            Map(x => x.RegistredIP, "RegistredIP").Not.Update();
            Map(x => x.Phone);
            Map(x => x.Postal);
            Map(x => x.RegistrationDate);
            Map(x => x.State);
            Map(x => x.Block);
            Map(x => x.PendingEmailAddress);
            Map(x => x.TokenCreationDate);
            Map(x => x.AccountResetToken, "UserResetToken");
            References(p => p.Country, "CountryId");
            References(p => p.Language, "LanguageId");
            References(p => p.Status, "StatusId");
            References(p => p.LastAccountLogin, "LastAccountLoginId").Cascade.None();
            HasMany(d => d.UserAccounts).KeyColumn("UserId").Cascade.All().Inverse();
            ApplyFilter<TenantFilter>();

        }
    }
}