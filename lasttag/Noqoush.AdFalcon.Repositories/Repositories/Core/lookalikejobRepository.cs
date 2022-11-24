using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;


namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
   

    public class lookalikejobRepository : RepositoryBase<lookalikejob, int>, IlookalikejobRepository
    {
        public lookalikejobRepository(RepositoryImplBase<lookalikejob, int> repository)
            : base(repository)
        {


        }
    }
}
