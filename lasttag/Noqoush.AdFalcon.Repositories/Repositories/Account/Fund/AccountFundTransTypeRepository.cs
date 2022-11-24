using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    class AccountFundTransTypeRepository : RepositoryBase<AccountFundTransType, int>, IAccountFundTransTypeRepository
    {
        public AccountFundTransTypeRepository(RepositoryImplBase<AccountFundTransType, int> repository)
            : base(repository)
        {
        }

    }
}
