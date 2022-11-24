using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account.SSP
{
    public interface ISSPPartnerSupportedCreativeFormatsRepository : IKeyedRepository<SSPPartnerSupportedCreativeFormats, int>
    {
    }
}

