using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class BusinessPartnerTypeRepository : RepositoryBase<BusinessPartnerType, int>, IBusinessPartnerTypeRepository
    {
        public BusinessPartnerTypeRepository(RepositoryImplBase<BusinessPartnerType, int> repository)
            : base(repository)
        {


        }
    }
}
