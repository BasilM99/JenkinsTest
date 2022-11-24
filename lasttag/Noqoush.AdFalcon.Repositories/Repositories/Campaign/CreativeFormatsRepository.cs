
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class CreativeFormatsRepository : RepositoryBase<CreativeFormat, int>, ICreativeFormatsRepository
    {   public CreativeFormatsRepository(RepositoryImplBase<CreativeFormat, int> repository)
            : base(repository)
        {
    }

}
    
}
