
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories.Core;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class CompanyTypeRepository : RepositoryBase<CompanyType, int>, ICompanyTypeRepository
    {
        public CompanyTypeRepository(RepositoryImplBase<CompanyType, int> repository)
            : base(repository)
        {


        }
    }
}
