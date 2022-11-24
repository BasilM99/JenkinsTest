using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
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