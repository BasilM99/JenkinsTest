using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class CountryVATMapping : ClassMap<CountryVAT>
    {
        public CountryVATMapping()
        {
            Table("country_vat");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'CountryVAT'");
            References(x => x.Country, "CountryId");
            Map(x => x.VATValue);

            Map(x => x.TaxNoRegistrationExpression);
            


        }
    }
}