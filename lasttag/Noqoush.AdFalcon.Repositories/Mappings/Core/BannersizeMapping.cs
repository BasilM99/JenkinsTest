using System;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Persistence.Mappings;

namespace Project.Model.Mappings
{
    public class BannersizeMapping : ClassMap<CreativeUnit>
    {
        public BannersizeMapping()
        {
            Table("bannersizes");

            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, 
                                           MappingSettings._nextHi, 
                                           MappingSettings._maxLo, 
                                           "TableKey = 'CreativeUnit'");
            Map(x => x.Description);
            Map(x => x.Height);
            References(x => x.Name, "NameId");
            Map(x => x.Width);
        }
    }
}