using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class AccountFeaturesMapping : ClassMap<AccountFeatures>
    {

        public AccountFeaturesMapping()
        {
            Table("account_features");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountFeatures'");
            References(x => x.Feature, "FeatureId");
            References(x => x.User, "UserId").LazyLoad();

            References(x => x.Account, "AccountId").LazyLoad();
            Map(x => x.DateNotify);
        }
    }
}