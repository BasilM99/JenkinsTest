using FluentNHibernate;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppsitesettingMapping : ClassMap<AppSiteSetting>
    {
        public AppsitesettingMapping()
        {
            Table("appsitesettings");
            Id(Reveal.Member<AppSiteSetting>("AppSiteId")).Column("AppSiteId").GeneratedBy.Foreign("AppSite");
            HasOne(p => p.AppSite).Constrained();


            Map(x => x.TestingModeId);
            Map(x => x.RefreshInterval);
            Map(x => x.RefreshModeId);
            BatchSize(200);
            //References(x => x.RefreshMode, "RefreshModeId");
        }
    }
}