
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class NativeAdLayoutMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.NativeAdLayout>
    {
        public NativeAdLayoutMapping()
        {
            Table("native_ad_layouts");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'NativeAdLayout'");
            References(r => r.Name, "NameId").Cascade.All(); 
            Map(x => x.Code);
            Map(x => x.Description);
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}
