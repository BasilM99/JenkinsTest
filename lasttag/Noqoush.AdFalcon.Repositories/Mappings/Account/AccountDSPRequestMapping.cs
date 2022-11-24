
using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;


namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{

    public class AccountDSPRequestMapping : ClassMap<AccountDSPRequest>
    {
        public AccountDSPRequestMapping()
        {
            Table("`account_dsp_request`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountDSPRequest'");
            // OptimisticLock.Version();

            Map(x => x.Address1);
            Map(x => x.Note);

            Map(x => x.Company);
            Map(x => x.EmailAddress);
            Map(x => x.FirstName);

            Map(x => x.LastName);

            Map(x => x.Phone);
            Map(x => x.ActionDate);
            Map(x => x.Status,"StatusId").CustomType<AccountDSPRequestStatus>(); ;
            Map(x => x.ActionNote);
            Map(x => x.RequestCode);
            Map(x => x.IsAllowNotifications);

            Map(x => x.RequestDate).Not.Update();

            References(p => p.Country, "CountryId").LazyLoad();
            References(p => p.Account, "AccountId").LazyLoad();
            References(p => p.CompanyType, "CompanyTypeId").LazyLoad();
            References(p => p.Approver, "ApproverId").LazyLoad();

        }
    }
}
