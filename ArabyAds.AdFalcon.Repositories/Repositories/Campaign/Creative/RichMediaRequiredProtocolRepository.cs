using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories.Campaign.Creative
{
    public class RichMediaRequiredProtocolRepository : RepositoryBase<RichMediaRequiredProtocol, int>, IRichMediaRequiredProtocolRepository
    {
        public RichMediaRequiredProtocolRepository(RepositoryImplBase<RichMediaRequiredProtocol, int> repository)
            : base(repository)
        {
        }
    }
}
