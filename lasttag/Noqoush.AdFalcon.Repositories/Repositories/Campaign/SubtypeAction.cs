using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    public class SubtypeActionRep : RepositoryBase<SubtypeAction, int>, ISubtypeAction
    {
        public SubtypeActionRep(RepositoryImplBase<SubtypeAction, int> repository)
            : base(repository)
        {
        }
    }
}
