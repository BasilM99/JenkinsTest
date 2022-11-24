using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class NativeAdIconMapping : ClassMap<NativeAdIcon>
    {
        public NativeAdIconMapping()
        {
            Table("native_ad_icons");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'NativeAdIcons'");
            Map(p => p.URL);
            References(p => p.MIMEType,"MIMETypeId");
            References(p => p.CreativeUnit, "CreativeUnitId");
            References(p => p.AdCreative, "NativeAdId");
            References(p => p.Document,"DocumentId");
        }
    }
}
