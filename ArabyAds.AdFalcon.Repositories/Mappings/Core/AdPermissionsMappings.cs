using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class AdPermissionsMappings : ClassMap<AdPermission>
    {
        public AdPermissionsMappings()
        {
            Table("`ad_permissions`");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdPermission'");
            Map(x => x.Code).CustomType<AdPermissionsCode>();
            References(x => x.Name, "NameId").Cascade.All();
        }

    }
}
