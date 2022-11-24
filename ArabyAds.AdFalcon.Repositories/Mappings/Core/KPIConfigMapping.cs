using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{

    public class KPIConfigMapping : ClassMap<KPIConfig>
    {
        public KPIConfigMapping()
        {
            Table("kpi_config");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'KPIConfig'");






            Map(x => x.ForAdvertiser);
            Map(x => x.ForDeals);
            Map(x => x.ForPublisher);
            Map(x => x.ForCampaign);
            Map(x => x.ForDataProvider);
            Map(x => x.DataBaseField);
            Map(x => x.IsDefault);
            Map(x => x.GrowIcon);
            Map(x => x.GroupKey);

            Map(x => x.Icon);
            Map(x => x.DisplayFormat);

            References(x => x.Name, "NameId").Cascade.All();
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }

}
