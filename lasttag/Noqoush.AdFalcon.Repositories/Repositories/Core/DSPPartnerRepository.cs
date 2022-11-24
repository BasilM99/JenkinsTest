using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
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
