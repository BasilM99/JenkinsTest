using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
	public class AgegroupMapping : ClassMap<AgeGroup>
	{
		public AgegroupMapping()
		{
			Table("agegroup");
            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AgeGroup'");
		    Map(x => x.MaxValue);
			Map(x => x.MinValue);
		    References(r => r.Name, "NameID");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
	}
}