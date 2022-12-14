using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class AdGroupCostElementMapping : ClassMap<AdGroupCostElement>
    {
        public AdGroupCostElementMapping()
        {

            Table("adgroup_cost_items");
            // ALi must me changed to TableKey ="AdGroupCostElement"
            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupCostElement'");
            Where("Type=1");
            Map(x => x.Value, "Value");
            Map(x => x.FromDate);
            Map(x => x.Type);
    
            
            Map(x => x.ToDate);
            Map(x => x.Scope).CustomType(typeof(AdGroupCostElementScope));
            References(r => r.AdGroup, "AdGroupId");
            References(r => r.CostElement, "CostItemId");
            References(r => r.Beneficiary, "BeneficiaryPartyId");
            References(x => x.CostModelWrapper, "CostModelWrapperId");
            References(x => x.Provider, "DataProviderId").Nullable().LazyLoad();

        }

    }


    public class AdGroupFeeMapping : ClassMap<AdGroupFee>
    {
        public AdGroupFeeMapping()
        {

            Table("adgroup_cost_items");
            // ALi must me changed to TableKey ="AdGroupCostElement"
            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdGroupCostElement'");
            Where("Type=2");
            Map(x => x.IsRemoved);
            Map(x => x.Value, "Value");
            Map(x => x.FromDate);
            Map(x => x.ToDate);
            Map(x => x.Type);
            Map(x => x.Scope).CustomType(typeof(AdGroupCostElementScope));
            References(r => r.AdGroup, "AdGroupId");
            References(r => r.Fee, "CostItemId");
            References(r => r.Beneficiary, "BeneficiaryPartyId").Not.Update();
            References(x => x.CostModelWrapper, "CostModelWrapperId");
            References(x => x.Provider, "DataProviderId").Nullable().LazyLoad();

        }

    }
}