using Noqoush.AdFalcon.Domain.Model.Account;
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
    public class AccountTrackingEventsRepository : RepositoryBase<AccountTrackingEvents, int>, IAccountTrackingEventsRepository
    {
        public AccountTrackingEventsRepository(RepositoryImplBase<AccountTrackingEvents, int> repository)
            : base(repository)
        {
        }

    }
}
