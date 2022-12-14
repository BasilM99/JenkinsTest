using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppsitesettingServerMapping : ClassMap<AppSiteServerSetting>
    {
        public AppsitesettingServerMapping()
        {
            Table("appsite_adserver_settings");

            Id(Reveal.Member<AppSiteServerSetting>("AppSiteId")).Column("AppSiteId").GeneratedBy.Foreign("AppSite");
            HasOne(Reveal.Member<AppSiteServerSetting, AppSite>("AppSite")).Constrained();

            Map(x => x.WatchTraffic);
            Map(x => x.GenerateSystemUniqueId);
            Map(x => x.ImpressionCountMode).CustomType<ImpressionCountMode>();
            Map(x => x.AllowBlindAds, "AllowBlindAds");
            Map(x => x.SupportedAdTypes);
            Map(x => x.SupportedBannerImageTypes);

            Map(x => x.AdRequestCacheLifeTime, "AdRequestCachedDataLifeTime");
            HasMany(p => p.Events).KeyColumn("AppSiteId").Cascade.AllDeleteOrphan().Inverse();

            References(x => x.NativeAdLayout, "NativeAdLayoutId").Cascade.None().Nullable();
            Map(x => x.RewardedVideoItemName);
            Map(x => x.RewardedVideoItemValue);
            Map(x => x.AppSitePlacementType, "PlacementType").CustomType(typeof(AppSitePlacementType));

        }
    }
}