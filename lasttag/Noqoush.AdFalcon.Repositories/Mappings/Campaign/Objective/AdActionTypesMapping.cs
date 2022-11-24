using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdActionTypesMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Objective.AdActionTypeBase>
    {
        public AdActionTypesMapping()
        {
            Table("adactiontypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdActionType'");
            References(x => x.Name, "NameID");
            Map(x => x.ShowInAppId, "ShowInAppId");
            Map(x => x.ViewName);
        Map(x => x.Code);
            //HasManyToMany(x => x.CostModelWrappers).Table("adactioncostmodelwrappers")
            //   .ParentKeyColumn("AdActionTypeId").ChildKeyColumn("CostModelWrapperId")
            //  .Fetch.Select()
            //  .AsSet().Cascade.All();
            HasMany(x => x.AdActionCostModelWrappers).KeyColumn("AdActionTypeId");
            HasMany(x => x.AdActionTrackingEvents).KeyColumn("AdActionTypeId");
            HasManyToMany(p => p.AdTypes).Table("adtypeactions").ParentKeyColumn("AdActionTypeId").ChildKeyColumn("AdTypeId").Fetch.Select().AsSet().Cascade.All();



            References(x => x.ActionImage, "ClickActionImageId");
            // HasOne(p => p.Constraints).PropertyRef(p => p.AdActionType).Cascade.All();
            HasMany(p => p.Constraints).KeyColumn("AdActionTypeId");
        }
    }
}
