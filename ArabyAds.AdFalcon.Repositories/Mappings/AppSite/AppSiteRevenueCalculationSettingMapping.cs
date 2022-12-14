using System;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class AppSiteRevenueCalculationSettingMapping : ClassMap<AppSiteRevenueCalculationSetting>
    {
        public AppSiteRevenueCalculationSettingMapping()
        {
            Table("appsite_revenue_calculation_modes");
            Id(x => x.ID)
                .GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo,
                                  "TableKey = 'AppSiteRevenueCalculationSetting'");
            References(x => x.AppSite, "AppSiteId").Cascade.All();
            Map(x => x.CalculationMode, "CalculationModeType").CustomType<CalculationMode>();
            Map(x => x.Value);
            Map(x => x.FromDate);
            Map(x => x.ToDate);
        }
    }
}