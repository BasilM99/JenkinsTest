using FluentNHibernate;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
  
    public class AccountInvitationMapping : ClassMap<AccountInvitation>
    {
        public AccountInvitationMapping()
        {
            Table("account_invitation");
            OptimisticLock.Version();
            Map(p => p.InvitationCode);
            Map(p => p.EmailAddress);
            Map(p => p.InvitationDate);
            Map(p => p.IsAccepted);
            Map(x => x.UserType, "UserType").CustomType<UserType>();
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                MappingSettings._nextHi,
                                MappingSettings._maxLo,
                                "TableKey = 'AccountInvitation'");
            References(x => x.Account, "AccountId");
                
                
                }
    }
}
