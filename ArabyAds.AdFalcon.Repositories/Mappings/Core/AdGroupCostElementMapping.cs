using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core.CostElement;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
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
            References(r => r.AdGroup, "AdGroupId").LazyLoad();
            References(r => r.CostElement, "CostItemId");
            References(r => r.Beneficiary, "BeneficiaryPartyId").LazyLoad();
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
            References(r => r.AdGroup, "AdGroupId").LazyLoad();
            References(r => r.Fee, "CostItemId");
            References(r => r.Beneficiary, "BeneficiaryPartyId").Not.Update().LazyLoad();
            References(x => x.CostModelWrapper, "CostModelWrapperId");
            References(x => x.Provider, "DataProviderId").Nullable().LazyLoad();

        }

    }
}