/*using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class FundTypeMapping : ClassMap<FundType>
    {
        public FundTypeMapping()
        {
            Table("`account_fund_trans_type`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'account_fund_trans_type'");
            Map(x => x.AllowImpersonate);
            Map(x => x.IsDeleted);
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();

        }
    }
}*/
