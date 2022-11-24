using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Persistence.Repositories.Campaign
{
    class AdCreativeAttributeRepository : RepositoryBase<Domain.Model.Campaign.AdCreativeAttribute, int>, IAdCreativeAttributeRepository
    {
        public AdCreativeAttributeRepository(RepositoryImplBase<Domain.Model.Campaign.AdCreativeAttribute, int> repository)
            : base(repository)
        {
        }
    }
}
