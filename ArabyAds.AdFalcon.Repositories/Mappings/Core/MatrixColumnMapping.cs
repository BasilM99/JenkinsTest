using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class metriceColumnMapping : ClassMap<metriceColumn>
    {
        public metriceColumnMapping()
        {
            Table("metrices_columns");
            Id(x => x.Id).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'metriceColumn'");
            Map(x => x.HeaderResourceKey);
            Map(x => x.HeaderResourceSet);
            Map(x => x.GroupKey);
            Map(x => x.DataBaseFieldName);
            Map(x => x.AppFieldName);
            Map(x => x.IsSelected);
            Map(x => x.DSP);
            Map(x => x.Publisher);
            Map(x => x.Format);
            Map(x => x.Advertiser);
            Map(x => x.Hide);
            Map(x => x.Order, "`order`");
        }
    }
}