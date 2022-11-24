using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class PortalPermisionMappings : ClassMap<PortalPermision>
    {
        public PortalPermisionMappings()
        {
            Table("`permissions`");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdPermission'");
            Map(x => x.Code).CustomType<PortalPermissionsCode>();
            References(x => x.Name, "NameId").Cascade.All();
            References(x => x.Categorie, "CategorieNameId").Cascade.All();
            BatchSize(100);
        }

    }
}
