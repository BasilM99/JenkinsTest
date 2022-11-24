using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class GenderRepository : RepositoryBase<Gender, int>, IGenderRepository
    {
        public GenderRepository(RepositoryImplBase<Gender, int> repository)
            : base(repository)
        {


        }
    }
}
