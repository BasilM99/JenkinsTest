using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.DomainServices.AuditTrial;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IAccountFeaturesRepository : IKeyedRepository<AccountFeatures, int>
    {
    
    }
}
