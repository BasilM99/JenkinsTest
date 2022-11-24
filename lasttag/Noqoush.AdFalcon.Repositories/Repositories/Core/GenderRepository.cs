using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class GenderRepository : RepositoryBase<Gender, int>, IGenderRepository
    {
        public GenderRepository(RepositoryImplBase<Gender, int> repository)
            : base(repository)
        {


        }
    }
}
