using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Account;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class BusinessPartnerAccountWhiteRepository : RepositoryBase<BusinessPartnerAccountWhite, int>, IBusinessPartnerAccountWhiteRepository
    {
        public BusinessPartnerAccountWhiteRepository(RepositoryImplBase<BusinessPartnerAccountWhite, int> repository)
            : base(repository)
        {
        }

    }
}
