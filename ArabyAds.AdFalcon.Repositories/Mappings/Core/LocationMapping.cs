using System.Runtime.Serialization;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
   

    public class LocationMapping : ClassMap<LocationBase>
    {
        public LocationMapping()
        {
            Table("`locations`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Location'");
            Map(x => x.CodeAlpha2,"Code_Alpha2");
            References(x => x.Name, "NameId").Cascade.All();
            References(x => x.Parent, "ParentId");
            HasMany(x => x.Locations).KeyColumn("ParentId").Inverse().LazyLoad().BatchSize(500).Cache.Transactional().ReadWrite();

            //Map(x => x.Type, "LocationType").CustomType(typeof(LocationType)).Not.Nullable();
            Map(x => x.TwoLettersCode, "Code1");
            Map(x => x.ThreeLettersCode, "Code2");
            Map(x => x.MobileCountryCode, "Code3");
            DiscriminateSubClassesOnColumn("LocationType");
            Cache.Transactional().ReadWrite().IncludeAll();
            BatchSize(500);
        }
    }

    public class CountryMapping : SubclassMap<Country>
    {
        public CountryMapping()
        {
            //this.DiscriminatorValue((int)LocationType.Country);
            this.DiscriminatorValue(2);
            BatchSize(500);
        }
    }

    public class ContinentMapping : SubclassMap<Continent>
    {
        public ContinentMapping()
        {
            //this.DiscriminatorValue((int)LocationType.Continent);
            this.DiscriminatorValue(1);
            BatchSize(500);
        }
    }

    public class CityMapping : SubclassMap<City>
    {
        public CityMapping()
        {
            //this.DiscriminatorValue((int)LocationType.City);
            this.DiscriminatorValue(4);
            BatchSize(500);
        }
    }


    public class StateMapping : SubclassMap<State>
    {
        public StateMapping()
        {
            //this.DiscriminatorValue((int)LocationType.State);
            this.DiscriminatorValue(3);
            BatchSize(500);
        }
    }
}