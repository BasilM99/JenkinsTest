
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    //campaigns
    public class DSPAccountSettingMapping : ClassMap<Noqoush.AdFalcon.Domain.Model.Account.DSPAccountSetting>
    {
        public DSPAccountSettingMapping()
        {
            Table("dsp_account_setting");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'DSPAccountSetting'");
            HasMany(p => p.Contacts).KeyColumn("AccountSettingId").Cascade.All().Inverse();

            References(x => x.Account, "AccountId").LazyLoad().Cascade.None();

            References(x => x.Country, "CountryId").Cascade.None();

            References(x => x.State, "StateId").Cascade.None();
            Map(p => p.BillToAddress2);
            Map(x => x.BillingContactName);
            Map(x => x.BillToAddressPersonName);
            Map(x => x.BillToAddress1);
            Map(x => x.BusinessName);

            // Map(x => x.IsDeleted);




        }
    }
}
