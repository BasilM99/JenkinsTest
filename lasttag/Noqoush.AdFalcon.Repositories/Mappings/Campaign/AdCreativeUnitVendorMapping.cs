using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{


    public class AdCreativeUnitVendorMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdCreativeUnitVendor>
    {
        public AdCreativeUnitVendorMapping()
        {
            Table("ad_creative_unit_vendors");

            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'AdCreativeUnitVendor'");

     
            References(x => x.Unit, "AdCreativeUnitId").LazyLoad();
            References(p => p.Vendor, "CreativeVendorId").LazyLoad();
    
        }
    }
}
