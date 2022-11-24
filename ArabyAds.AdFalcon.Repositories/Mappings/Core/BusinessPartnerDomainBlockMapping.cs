using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    public class BusinessPartnerDomainBlockMapping : ClassMap<BusinessPartnerDomainBlock>
    {
        public BusinessPartnerDomainBlockMapping()
        {


            Table("business_partner_domain_block");
            Id(x => x.ID, "Id").GeneratedBy.Identity();
            Map(M=>M.Domain);
            References(x => x.Partner, "PartnerId").Cascade.None();


        }
    }
}