﻿using FluentNHibernate.Mapping;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign.Objective
{
    public class AdActionTypeConstraintMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Objective.AdActionTypeConstraint>
    {
        public AdActionTypeConstraintMapping()
        {
            Table("actiontypeconstraints");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdActionTypeConstraint'");
            References(x => x.Name, "NameID");
            References(x => x.Platform, "PlatformID");
            References(x => x.AdActionType, "AdActionTypeId");
            Map(x => x.DeviceConstraint, "SubType");
        }
    }
}
