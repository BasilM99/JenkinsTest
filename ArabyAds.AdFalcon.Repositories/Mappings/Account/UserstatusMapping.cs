using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class UserStatusMapping : ClassMap<UserStatus>
    {
        public UserStatusMapping()
        {
            Table("`userstatus`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'UserStatus'");

            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}