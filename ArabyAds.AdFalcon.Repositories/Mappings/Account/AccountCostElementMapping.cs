
using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;


namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{

    public class AccountCostElementMapping : ClassMap<AccountCostElement>
    {
        public AccountCostElementMapping()
        {
            Table("`account_cost_element`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountCostElement'");
            // OptimisticLock.Version();

            Map(x => x.Enabled);
            //Map(x => x.CostValue);
            References(p => p.Account, "AccountId").LazyLoad();
            References(p => p.CostElement, "CostElementId").Not.LazyLoad();
            References(p => p.DataProvider, "DataProviderId").LazyLoad();
            References(r => r.Beneficiary, "BeneficiaryPartyId");
        }
    }


    public class AccountFeeMapping : ClassMap<AccountFee>
    {
        public AccountFeeMapping()
        {
            Table("`account_fee`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountFee'");
            // OptimisticLock.Version();

            Map(x => x.Enabled);
            //Map(x => x.CostValue);
            
            References(p => p.Fee,"FeeId").Not.LazyLoad();
            References(p => p.Account, "AccountId").LazyLoad();
            References(r => r.Beneficiary, "BeneficiaryPartyId");
        }
    }
}
