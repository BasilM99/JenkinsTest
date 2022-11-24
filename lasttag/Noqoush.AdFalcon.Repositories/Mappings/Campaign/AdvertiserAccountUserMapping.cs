using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    public class AdvertiserAccountUserMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Campaign.AdvertiserAccountUser>
    {
        public AdvertiserAccountUserMapping()
        {
            Table("advertiser_account_assignment");
            Id(x => x.ID, "Id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                                MappingSettings._maxLo, "TableKey = 'AdvertiserAccountUser'");

            //Map(x => x.Name);
            //References(x => x.Account, "AccountId").LazyLoad();
            References(x => x.User, "UserId").LazyLoad();
            References(x => x.Link, "AssociationId").LazyLoad();
            Map(x => x.Read, "IsRead");
            Map(x => x.Write, "IsWrite");
            References(x => x.Invitation, "InvitationId").LazyLoad();
            Map(x => x.IsDeleted);

        }
    }



   
}
