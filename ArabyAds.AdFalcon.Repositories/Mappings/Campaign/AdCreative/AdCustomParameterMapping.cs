using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign.AdCreative
{
    public class AdCustomParameterMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdCustomParameter>
    {
        public AdCustomParameterMapping()
        {
            Table("ad_custom_parameters");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AdCustomParameters'");
            Map(p => p.Name, "Name");
            Map(p => p.Value, "Value");
            Map(p => p.IsDeleted, "IsDeleted");
            Map(p => p.IsMandatory, "IsMandatory");
            References(p => p.AdCreative, "AdId");
        }
    }
}
