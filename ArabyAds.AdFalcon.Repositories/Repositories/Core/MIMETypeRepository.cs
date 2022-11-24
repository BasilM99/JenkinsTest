using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
    public class MIMETypeRepository : RepositoryBase<MIMEType, int>, IMIMETypeRepository
    {
        public MIMETypeRepository(RepositoryImplBase<MIMEType, int> repository)
            : base(repository)
        {


        }
    }
}
