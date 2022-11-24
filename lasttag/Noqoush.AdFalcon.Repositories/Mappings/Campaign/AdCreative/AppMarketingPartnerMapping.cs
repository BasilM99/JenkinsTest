using  FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;


namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{

    public class AppMarketingPartnerMapping : ClassMap<AppMarketingPartner>
    {
        public AppMarketingPartnerMapping()
        {
            Table("app_marketing_partners");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AppMarketingPartner'");
            Map(X=>X.Code);
            Map(X => X.Description);
          //  References(x => x.Name, "NameId").Not.LazyLoad().Fetch.Join().Cascade.All();
            References(x => x.Name, "NameId").Cascade.All();
              HasMany(x => x.Trackers).KeyColumn("AppMarketingPartnerId").Not.LazyLoad().Cascade.All();
              HasOne(Reveal.Member<AppMarketingPartner, Noqoush.AdFalcon.Domain.Model.AppSite.AppSite>("AppSite")).Constrained().ForeignKey("AppSiteId").Constrained();
        }
    }
}
