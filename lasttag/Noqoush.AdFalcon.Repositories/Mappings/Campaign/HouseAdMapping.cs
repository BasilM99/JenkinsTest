using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class HouseAdMapping : ClassMap<HouseAd>
    {
        public HouseAdMapping()
        {
            Table("house_ads");
            //Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'HouseAds");
            Map(x => x.IsDeleted, "IsDeleted");
            Map(x => x.DeliveryMode, "DeliveryMode").CustomType(typeof(HouseAdDeliveryMode));
            References(x => x.Account, "AccountId");
            References(x => x.ForAppSite, "ForAppsiteId");
            References(x => x.User, "UserId");
            //References(x => x.AdGroup, "AdGroupId");

            /* HasManyToMany(x => x.DestinationAppSites)
                 .Table("house_ad_appsites")
                 .ParentKeyColumn("house_ad_id")
                 .ChildKeyColumn("appsite_id")
                 .Cascade.AllDeleteOrphan().Inverse();
             */

            HasManyToMany(x => x.DestinationAppSites)
               .ChildKeyColumn("appsite_id")
               .ParentKeyColumn("house_ad_id")
               .Table("house_ad_appsites")
               .Fetch.Select()
               .AsSet().Cascade.All();

            Id(Reveal.Member<HouseAd>("AdGroupId")).Column("AdGroupId").GeneratedBy.Foreign("AdGroup");
            HasOne(Reveal.Member<HouseAd, AdGroup>("AdGroup")).Constrained().ForeignKey("AdGroupId").Constrained();

        }
    }
}