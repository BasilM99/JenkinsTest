using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core
{
  

    public class SSPPartnerWhileIPRepository : RepositoryBase<SSPPartnerWhiteIP, int>, ISSPPartnerWhileIPRepository
    {
        public SSPPartnerWhileIPRepository(RepositoryImplBase<SSPPartnerWhiteIP, int> repository)
            : base(repository)
        {
        }

    }
}
