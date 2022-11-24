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
   

    public class PixelMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.Pixel>
    {
        public PixelMapping()
        {
            Table("pixels");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'pixel'");

            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.Link, "AccountAdvAssId");
          
            Map(x => x.Status).CustomType(typeof(PixelStatus));
            Map(x => x.Code).Not.Update();
            Map(x => x.Name);

            Map(x => x.IsDeleted);
            HasMany(x => x.AudienceSegmentListsMap).KeyColumn("PixelId").Cascade.AllDeleteOrphan().Inverse();
            //HasMany(d => d.Items).KeyColumn("LinkId").Cascade.All().Inverse();
        }
    }
}
