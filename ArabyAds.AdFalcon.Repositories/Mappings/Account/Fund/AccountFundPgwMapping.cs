using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Persistence.Mappings;

namespace ArabyAds.AdFalcon.Repositories.Mappings.Account
{
    public class AccountFundPgwMapping : ClassMap<AccountFundPgw>
    {
        public AccountFundPgwMapping()
        {
            Table("`account_fund_pgw`");
            Id(x => x.ID, "id").GeneratedBy.HiLo(MappingSettings.HiLowTableName, MappingSettings._nextHi, MappingSettings._maxLo, "TableKey = 'AccountFundPgw'");
            Map(x => x.ApiResolver, "api_resolver");
           // Map(x => x.OutLetId);
            //Map(x => x.ApiRef);
          
            Map(x => x.IntegrationPageUrl, "integration_page_url");
            Map(x => x.IsDeleted, "is_deleted");
            Map(x => x.ReturnPageUrl, "return_page_url");
            Map(x => x.ConfigData, "config_data");
            References(x => x.Name, "name_id");
            Map(p => p.Code);

        }
    }
}