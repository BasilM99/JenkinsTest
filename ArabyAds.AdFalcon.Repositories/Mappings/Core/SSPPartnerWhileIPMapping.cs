using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ArabyAds.AdFalcon.Persistence.Mappings.Core
{
    
    public class SSPPartnerWhileIPMapping : ClassMap<SSPPartnerWhiteIP>
    {
        public SSPPartnerWhileIPMapping()
        {
            Table("ssp_partner_whitelist_ips");
            Id(x => x.ID, "Id").GeneratedBy.Identity();
            Map(x => x.IP, "IP");
            References(x => x.SSPPartner, "PartnerId").Cascade.None();

        }
    }
}
