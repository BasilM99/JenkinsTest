using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
    public class CampaignAssignedAppsitesMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.CampaignAssignedAppsite>
    {
        public CampaignAssignedAppsitesMapping()
        {
            Table("adgroup_assigned_appsites");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, 
                                                MappingSettings._maxLo, "TableKey = 'CampaignAssignedAppsite'");

            Where("AdGroupId IS NULL");
            References(x => x.Campaign, "CampaignId");
            References(x => x.AppSite, "AppsiteId");
            Map(x => x.SubPublisherId).Nullable().Formula("(SELECT sub_appsites.SubPublisherId FROM sub_appsites where sub_appsites.Id=SubAppSiteId)").Not.Insert().Not.Update(); 
            References(x => x.Account, "AppSiteAccountId").LazyLoad();
            Map(x => x.IsDeleted);
            Map(x => x.Include);
            References(x => x.SubAppsite, "SubAppSiteId").LazyLoad();
        }
    }
}
