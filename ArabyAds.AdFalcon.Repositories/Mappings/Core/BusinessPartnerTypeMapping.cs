using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class BusinessPartnerTypeMapping : ClassMap<BusinessPartnerType>
    {
        public BusinessPartnerTypeMapping()
        {
            Table("business_partner_type");

            Id(x => x.ID, "ID").GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'BusinessPartnerType'");

            References(x => x.Name, "NameId").Cascade.All();
            Map(X => X.Code);
            Cache.Transactional().ReadWrite().IncludeAll();


        }
    }
}