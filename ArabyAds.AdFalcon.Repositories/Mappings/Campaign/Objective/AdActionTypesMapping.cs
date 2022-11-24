using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdActionTypesMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.Objective.AdActionTypeBase>
    {
        public AdActionTypesMapping()
        {
            Table("adactiontypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdActionType'");
            References(x => x.Name, "NameID");
            Map(x => x.ShowInAppId, "ShowInAppId");
            Map(x => x.ViewName);
            Map(x => x.ShowForObjective);
            Map(x => x.Code);
            //HasManyToMany(x => x.CostModelWrappers).Table("adactioncostmodelwrappers")
            //   .ParentKeyColumn("AdActionTypeId").ChildKeyColumn("CostModelWrapperId")
            //  .Fetch.Select()
            //  .AsSet().Cascade.All();
            HasMany(x => x.AdActionCostModelWrappers).KeyColumn("AdActionTypeId").Cache.ReadWrite();
            HasMany(x => x.AdActionTrackingEvents).KeyColumn("AdActionTypeId").Cache.ReadWrite(); ;
            HasManyToMany(p => p.AdTypes).Table("adtypeactions").ParentKeyColumn("AdActionTypeId").ChildKeyColumn("AdTypeId").Fetch.Select().AsSet().Cascade.All().Cache.ReadWrite();



            References(x => x.ActionImage, "ClickActionImageId");
            // HasOne(p => p.Constraints).PropertyRef(p => p.AdActionType).Cascade.All();
            HasMany(p => p.Constraints).KeyColumn("AdActionTypeId").Cache.ReadWrite();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
