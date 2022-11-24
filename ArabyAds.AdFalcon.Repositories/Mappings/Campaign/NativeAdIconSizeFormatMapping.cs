using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class NativeAdIconSizeFormatMapping : ClassMap<NativeAdIconSizeFormat>
    {
        public NativeAdIconSizeFormatMapping()
        {
            Table("native_ad_icon_sizes_formats");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.Format);
            Map(p => p.MaxSize);
            References(p => p.IconSize, "NativeAdIconSizeId");
        }
    }
}
