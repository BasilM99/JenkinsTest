using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class NativeAdImageSizeMapping : ClassMap<NativeAdImageSize>
    {
        public NativeAdImageSizeMapping()
        {
            Table("native_ad_image_sizes");
            Id(p => p.ID).GeneratedBy.Identity();
            Map(p => p.Width);
            Map(p => p.Height);
            Map(p => p.Code);
            Map(p => p.Priority);
            Map(p => p.IsRequired);
        }
    }
}
