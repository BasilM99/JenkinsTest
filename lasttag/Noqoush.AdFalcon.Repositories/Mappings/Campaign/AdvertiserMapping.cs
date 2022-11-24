using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign;


namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{

    public class AdvertiserMapping : ClassMap< Advertiser>
    {
        public AdvertiserMapping()
        {
            Table("advertisers");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'Advertiser'");
            References(x => x.Name, "NameID").Cascade.All().Not.LazyLoad();
       
            //Map(X => X.AdvertiserBusinessId);
            Map(X => X.Description).Nullable();
            Map(X => X.DomainURL).Nullable();
            Map(X => X.UniqueId).Not.Update();
            
            Cache.Transactional().ReadWrite().IncludeAll();
    
        }
    }
}
