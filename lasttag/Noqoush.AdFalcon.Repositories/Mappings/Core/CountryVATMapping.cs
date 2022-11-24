using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
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