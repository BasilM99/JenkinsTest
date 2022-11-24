using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignBidConfigMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdGroupBidConfig>
    {
        public CampaignBidConfigMapping()
        {
            Table("adgroup_bid_config");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'CampaignBidConfig'");
            References(p => p.AdGroup, "AdGroupId");  
            References(x => x.Account, "AccountId");
            References(x => x.AppSite, "AppsiteId");
            References(x => x.SubAppSite, "SubAppSiteId").LazyLoad();

            Map(x => x.SubPublisherId).Nullable().Formula("(SELECT sub_appsites.SubPublisherId FROM sub_appsites where sub_appsites.Id=SubAppSiteId)").Not.Insert().Not.Update();
            Map(x => x.IsDeleted);
            Map(x => x.Bid);
        }
    }
}
