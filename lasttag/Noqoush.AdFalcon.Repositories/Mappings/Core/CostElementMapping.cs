using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class CostItemMapping : ClassMap<CostItem>
    {
        public CostItemMapping()
        {
            Table("cost_items");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CostElement'");
            References(x => x.Name, "NameId").Cascade.All();

            Map(x => x.Category);
            Map(x => x.CostItemType,"Type").CustomType<CostItemType>();
            Map(x => x.Type,"CalculationType").CustomType<CalculationType>();
            HasMany(x => x.Values).KeyColumn("CostItemId").Cascade.All();
        }
    }
    public class CostElementMapping : SubclassMap<CostElement>
    {
        public CostElementMapping()
        {
            Table("cost_elements");
           
            KeyColumn("Id");

          Map(x => x.IsOneTime);

            Map(x => x.Scope);
            Map(x => x.CalculatedFromFeeCategory);
        Map(x => x.CostElementCalculatedFrom, "CalculatedFrom").CustomType<CostElementCalculatedFrom>();
         
        }
    }
    public class FeeMapping : SubclassMap<Fee>
    {
        public FeeMapping()
        {
            Table("fees");
            KeyColumn("Id");
            Map(x => x.IsAutoAdded);
            Map(x => x.IsBillable);
            Map(x => x.FeeCalculatedFrom, "CalculatedFrom").CustomType<FeeCalculatedFrom>();
        }
    }

    public class CostItemValueMapping : ClassMap<CostItemValue>
    {
        public CostItemValueMapping()
        {
            Table("cost_items_values_");
            Id(x => x.ID).GeneratedBy.Identity();
            Map(x => x.Value);
            References(x => x.CostModelWrapper, "CostModelWrapperId");
        }
    }
}