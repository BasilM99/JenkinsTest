using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class BusinessPartnerAccountWhiteMapping : ClassMap<BusinessPartnerAccountWhite>
    {
        public BusinessPartnerAccountWhiteMapping()
        {


            Table("business_partner_account_white");
            Id(x => x.ID, "Id").GeneratedBy.Identity();
            References(x => x.Account, "AccountId").Cascade.None();
            References(x => x.Partner, "PartnerId").Cascade.None();


        }
    }
}