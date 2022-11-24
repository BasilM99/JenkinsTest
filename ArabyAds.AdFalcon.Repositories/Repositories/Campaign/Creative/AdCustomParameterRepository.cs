using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{

    public class AdCustomParameterRepository : RepositoryBase<Domain.Model.Campaign.AdCustomParameter, int>, IAdCustomParameterRepository
    {
        public AdCustomParameterRepository(RepositoryImplBase<Domain.Model.Campaign.AdCustomParameter, int> repository)
            : base(repository)
        {
        }
    }
}
