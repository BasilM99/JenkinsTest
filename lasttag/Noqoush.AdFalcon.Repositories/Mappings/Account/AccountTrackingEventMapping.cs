using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class AccountTrackingEventMapping : ClassMap<AccountTrackingEvents>
    {

        public AccountTrackingEventMapping()
        {
            Table("account_events");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi,
                                           MappingSettings._maxLo, "TableKey = 'AccountTrackingEvents'");
            Map(x => x.Code);
            Map(x => x.Description, "Name");
            Map(x => x.UserID);
            Map(x => x.AccountId);
            Map(x => x.IsConversion);
        }

    }
}
