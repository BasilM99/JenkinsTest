﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;


namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
   
    public class AdRequestPlatformMapping : ClassMap<AdRequestPlatform>
    {
        public AdRequestPlatformMapping()
        {
            Table("adrequest_version_platforms");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AdRequestPlatform'");
            References(x => x.Name, "NameID");
            Map(X => X.Code);
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}
