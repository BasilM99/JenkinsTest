using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class NativeAdIconSizeMapping : ClassMap<NativeAdIconSize>
    {
        public NativeAdIconSizeMapping()
        {
            Table("native_ad_icon_sizes");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.Width);
            Map(p => p.Height);
            Map(p => p.Code);
            Map(p => p.Priority);
            Map(p => p.IsRequired);
            HasMany(p => p.Formats).KeyColumn("NativeAdIconSizeId").Cascade.All();
        }
    }
}
