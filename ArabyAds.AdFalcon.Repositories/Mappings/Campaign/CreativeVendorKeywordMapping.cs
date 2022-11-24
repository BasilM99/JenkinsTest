using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CreativeVendorKeywordMapping : ClassMap<CreativeVendorKeyword>
    {
        public CreativeVendorKeywordMapping()
        {


            Table("creative_vendor_keywords");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CreativeVendorKeyword'");
            References(x => x.Vendor, "CreativeVendorId").Cascade.None();
            Map(x => x.Keyword);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
