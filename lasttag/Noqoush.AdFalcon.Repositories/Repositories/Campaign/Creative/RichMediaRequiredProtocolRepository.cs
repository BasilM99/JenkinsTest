using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class RichMediaRequiredProtocolRepository : RepositoryBase<RichMediaRequiredProtocol, int>, IRichMediaRequiredProtocolRepository
    {
        public RichMediaRequiredProtocolRepository(RepositoryImplBase<RichMediaRequiredProtocol, int> repository)
            : base(repository)
        {
        }
    }
}
