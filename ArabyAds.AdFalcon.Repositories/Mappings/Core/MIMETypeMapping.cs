using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class MIMETypeMapping : ClassMap<MIMEType>
    {
        public MIMETypeMapping()
        {
            Table("mime_types");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'MIMETypes'");
            Map(p => p.MIME);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
