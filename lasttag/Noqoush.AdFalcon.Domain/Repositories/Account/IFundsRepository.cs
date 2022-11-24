using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IFundsRepository : IKeyedRepository<AccountFundTransHistory, int>
    {
    }
}
