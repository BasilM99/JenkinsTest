﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IAccountFundTransHistoryRepository : IKeyedRepository<AccountFundTransHistory, int>
    {

    }
}
