using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArabyAds.AdFalcon.Persistence.Mappings.Campaign
{
   

    public class AdGroupInventorySourceMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Campaign.AdGroupInventorySource>
    {
        public AdGroupInventorySourceMapping()
        {
            Table("adgroup_assigned_appsites");
       
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'CampaignAssignedAppsite'");

            Where("AdGroupId > 0");
            References(x => x.Campaign, "CampaignId").LazyLoad();
            References(p => p.AdGroup, "AdGroupId").LazyLoad();
            References(x => x.Account, "AppSiteAccountId").LazyLoad();
            References(x => x.Partner, "SSPId");
            References(x => x.AppSite, "AppsiteId");
            References(x => x.SubAppsite, "SubAppSiteId").LazyLoad();
            Map(x => x.SubPublisherId).Nullable().Formula("(SELECT sub_appsites.SubPublisherId FROM sub_appsites where sub_appsites.Id=SubAppSiteId)").Not.Insert().Not.Update();
            //Map(x => x.SubPublisherId);
            Map(x => x.IsDeleted);
            Map(x => x.Include);



        }
    }
}
