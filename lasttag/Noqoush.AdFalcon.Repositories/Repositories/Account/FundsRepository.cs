using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    class FundsRepository : RepositoryBase<AccountFundTransHistory, int>, IFundsRepository
    {
        public FundsRepository(RepositoryImplBase<AccountFundTransHistory, int> repository)
            : base(repository)
        {
        }
    }
}
