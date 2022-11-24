using FluentNHibernate.Mapping;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdGroupObjectiveTypeMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.Objective.AdGroupObjectiveType>
    {
        public AdGroupObjectiveTypeMapping()
        {
            Table("adgroupobjectivetypes");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupObjectiveType'");
            References(x => x.Name, "NameID");
            References(x => x.Descrption, "DescrptionId");
            HasManyToMany(x => x.AdActionTypes)
              .ChildKeyColumn("AdActionTypeId")
              .ParentKeyColumn("AdGroupObjectiveTypeId")
              .Table("adgroupobjectivetypeactions")
              .Fetch.Select()
              .AsSet().Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
