using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
    public class MIMETypeRepository : RepositoryBase<MIMEType, int>, IMIMETypeRepository
    {
        public MIMETypeRepository(RepositoryImplBase<MIMEType, int> repository)
            : base(repository)
        {


        }
    }
}
