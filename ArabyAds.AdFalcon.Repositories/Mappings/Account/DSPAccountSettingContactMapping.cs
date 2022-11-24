
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    //campaigns
    public class DSPAccountSettingContactMapping : ClassMap<ArabyAds.AdFalcon.Domain.Model.Account.DSPAccountSettingContact>
    {
        public DSPAccountSettingContactMapping()
        {
            Table("dsp_account_setting_contact");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'DSPAccountSettingContact'");
            Map(p => p.Email);
            Map(p => p.IsDeleted);
            References(p => p.DSPAccountSetting, "AccountSettingId").LazyLoad().Cascade.None();

            // Map(x => x.IsDeleted);




        }
    }
}
