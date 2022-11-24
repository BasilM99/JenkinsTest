using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.AppSite
{
    public class SubAppsiteMapping : ClassMap<SubAppsite>
    {
        public SubAppsiteMapping()
        {
            Table("sub_appsites");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'SubAppsite'");
            Map(x => x.SubPublisherId);
            Map(x => x.SubPublisherMarketId);
            Map(x => x.SubPublisherName);
            Map(x => x.SubPublisherUrl);
            References(x => x.AppSite, "AppsiteId").Cascade.All().LazyLoad();
        }
    }
}
