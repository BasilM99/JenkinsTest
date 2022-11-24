using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories.Core;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Core
{
   

    public class DSPPartnerRepository : RepositoryBase<DSPPartner, int>, IDSPPartnerRepository
    {
        public DSPPartnerRepository(RepositoryImplBase<DSPPartner, int> repository)
            : base(repository)
        {


        }
    }

    public class DPPartnerRepository : RepositoryBase<DPPartner, int>, IDPPartnerRepository
    {
        public DPPartnerRepository(RepositoryImplBase<DPPartner, int> repository)
            : base(repository)
        {


        }
    }

}
