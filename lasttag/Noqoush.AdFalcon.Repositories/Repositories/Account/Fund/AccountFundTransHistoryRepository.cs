using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;
using NHibernate;


namespace Noqoush.AdFalcon.Persistence.Repositories
{
    public class AccountFundTransHistoryRepository : RepositoryBase<AccountFundTransHistory, int>, IAccountFundTransHistoryRepository
    {
        public AccountFundTransHistoryRepository(RepositoryImplBase<AccountFundTransHistory, int> repository)
            : base(repository)
        {
        }
       
    }
}
