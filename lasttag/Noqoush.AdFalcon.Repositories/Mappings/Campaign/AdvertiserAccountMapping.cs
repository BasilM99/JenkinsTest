using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdvertiserAccountMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdvertiserAccount>
    {
        public AdvertiserAccountMapping()
        {
            Table("advertiser_account");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'AdvertiserAccount'");

          
            References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.Advertiser, "AdvertiserId");
            Map(x => x.IsRestricted);
            Map(x=>x.Name);
            Map(x => x.IsDeleted);
        
            HasMany(d => d.Users).KeyColumn("AssociationId").Cascade.All();
        
            Map(X => X.AgencyCommission, "AgencyCommissionModel").CustomType(typeof(AgencyCommission)).Nullable(); 
            Map(X => X.AgencyCommissionValue, "AgencyCommissionModelValue");
        }
    }
}
