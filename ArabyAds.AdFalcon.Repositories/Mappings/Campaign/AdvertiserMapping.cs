using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Persistence.Mappings.Tenant;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
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
            References(x => x.Tenant, "TenantId").Not.Update();
            ApplyFilter<TenantFilter>();
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(500);

        }
    }
}
