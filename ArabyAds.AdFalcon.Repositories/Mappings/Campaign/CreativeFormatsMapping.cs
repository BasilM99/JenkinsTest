using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Performance;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CreativeFormatsMapping : ClassMap<CreativeFormat>
    {
        public CreativeFormatsMapping()
        {
            Table("creative_formats");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CreativeFormats'");
            References(x => x.Name, "NameId").Cascade.All();
            Map(x => x.Description);
            Map(x => x.Code);
            //Map(x => x.IsDeleted);
        }
    }
}
