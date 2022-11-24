using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdvertiserAccountMasterAppSiteItemMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdvertiserAccountMasterAppSiteItem>
    {
        public AdvertiserAccountMasterAppSiteItemMapping()
        {
            Table("content_list_appsites");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'masterappsiteitem'");


            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.Link, "LinkId");
            Map(x => x.Code);
            Map(x => x.Type).CustomType(typeof(MasterAppSiteItemType));
            Map(x => x.BundleID, "AppID");
            Map(x => x.AppSiteName);
            Map(x => x.Domain);
            Map(x => x.IsDeleted);
          
        }
    }
}
