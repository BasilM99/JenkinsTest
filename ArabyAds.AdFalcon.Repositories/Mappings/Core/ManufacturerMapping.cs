using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
	public class ManufacturerMapping : ClassMap<Manufacturer>
	{
		public ManufacturerMapping()
		{
			Table("manufacturers");
            Id(x => x.ID, "ManufacturerId").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Manufacturer'");
            Map(x => x.Order, "`order`");
            References(x => x.Name, "NameId").Cascade.All(); ;
		}
	}
}