using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdvertiserAccountMasterAppSiteMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdvertiserAccountMasterAppSite>
    {
        public AdvertiserAccountMasterAppSiteMapping()
        {
            Table("content_list");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'masterappsite'");

            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.Link, "AccountAdvAssId");
            Map(x => x.Type).CustomType(typeof(MasterAppSiteType));
            Map(x => x.Status).CustomType(typeof(MasterAppSiteStatus));
            Map(x => x.GlobalScope);
            Map(x => x.Name);
        
            Map(x => x.IsDeleted);
            Map(x => x.LastModifiedDate);
            HasMany(d => d.Items).KeyColumn("LinkId").Cascade.All().Inverse();
        }
    }
}
