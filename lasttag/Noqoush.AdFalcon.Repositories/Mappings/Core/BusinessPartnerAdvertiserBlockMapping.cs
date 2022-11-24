using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Core
{
    public class BusinessPartnerAdvertiserBlockMapping : ClassMap<BusinessPartnerAdvertiserBlock>
    {
        public BusinessPartnerAdvertiserBlockMapping()
        {


            Table("business_partner_advertiser_block");
            Id(x => x.ID, "Id").GeneratedBy.Identity();
            References(x => x.Advertiser, "AdvertiserId").Cascade.None();
            References(x => x.Partner, "PartnerId").Cascade.None();


        }
    }
}