using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppsiteMapping : ClassMap<AppSite>
    {
        public AppsiteMapping()
        {
            Table("appsite");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AppSite'");
            References(x => x.Account, "AccountId");
            References(x => x.User, "UserId");

            Map(x => x.PublisherId);
            Map(x => x.Description);
            Map(x => x.Name);
            Map(x => x.IsPublished);
            Map(x => x.AdminComments);
            Map(x => x.RegistrationDate);
            Map(x => x.IsDeleted);
           // Map(x => x.NameLower);

            References(x => x.Theme, "ThemeId").Cascade.All().Not.LazyLoad();
            HasOne(p => p.AppSiteSetting).Not.LazyLoad().Constrained().Cascade.SaveUpdate();
            
            //References(p => p.AppSiteServerSetting).PropertyRef(x => x.AppSite);
            //HasOne(p => p.AppSiteServerSetting).PropertyRef(p => p.AppSite).Cascade.All();
            //HasMany(d => d.AppSiteServerSettings).KeyColumn("AppSiteId").Cascade.All();
            //HasOne(d => d.AppSiteServerSetting).LazyLoad().PropertyRef(x => x.AppSite).Cascade.All();
            HasOne(d => d.AppSiteServerSetting).LazyLoad().ForeignKey("Id").Cascade.SaveUpdate().Constrained();
            HasMany(d => d.RevenueCalculationSettings).KeyColumn("AppSiteId").Cascade.All();
            References(x => x.Type, "TypeId").Not.LazyLoad();
            References(x => x.Status, "StatusId").Not.LazyLoad(); 
            HasMany(p => p.AppSiteFilters).KeyColumn("appsiteid").Cascade.All();
            HasMany(p => p.SubAppsites).KeyColumn("AppsiteId").Cascade.All();
            //HasManyToMany(x => x.Keywords)
            //    .ChildKeyColumn("KeywordId")
            //    .ParentKeyColumn("AppSiteId")
            //    .Table("appsitekeywords")
            //    .Fetch.Select()
            //    .AsSet().Cascade.All();

            
            HasMany(d => d.Keywords).KeyColumn("AppSiteId").Fetch.Select().Not.KeyNullable().Cascade.AllDeleteOrphan();
            
            //.Formula("case when IsApp is null then '0' else '1' end"); ;
            DiscriminateSubClassesOnColumn<sbyte>("IsApp", (sbyte)-1);
        }
    }
    public class AppMapping : SubclassMap<App>
    {
        public AppMapping()
        {
            DiscriminatorValue(1);
            Map(x => x.IsUseLocation);
            Map(x => x.MarketURL, "URL");
            Map(x => x.SubType, "SubType").Nullable();
        }
    }
    public class SiteMapping : SubclassMap<Site>
    {
        public SiteMapping()
        {
            DiscriminatorValue(0);
            Map(x => x.SiteURL, "URL");
        }
    }
}