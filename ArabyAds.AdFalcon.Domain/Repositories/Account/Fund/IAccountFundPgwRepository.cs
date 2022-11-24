using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IAccountFundPgwRepository  : IKeyedRepository<AccountFundPgw, int>
    {
    }
}
