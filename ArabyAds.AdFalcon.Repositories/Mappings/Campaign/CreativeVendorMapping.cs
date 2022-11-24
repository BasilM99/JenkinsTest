using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CreativeVendorMapping : ClassMap<CreativeVendor>
    {
        public CreativeVendorMapping()
        {


            Table("creative_vendors");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CreativeVendor'");
            References(x => x.Name, "NameId").Cascade.All();
            HasMany(x => x.Keywords).KeyColumn("CreativeVendorId").Cascade.AllDeleteOrphan().BatchSize(500);
            Map(x=>x.Code);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
