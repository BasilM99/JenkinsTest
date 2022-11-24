using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;


namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
   

    public class lookalikejobRepository : RepositoryBase<lookalikejob, int>, IlookalikejobRepository
    {
        public lookalikejobRepository(RepositoryImplBase<lookalikejob, int> repository)
            : base(repository)
        {


        }
    }
}
