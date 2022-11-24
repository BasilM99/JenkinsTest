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
    class AccountFundPgwRepository : RepositoryBase<AccountFundPgw, int>, IAccountFundPgwRepository
    {
        public AccountFundPgwRepository(RepositoryImplBase<AccountFundPgw, int> repository)
            : base(repository)
        {
        }

    }
}
