using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Core.Video;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Account;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core.Video;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Persistence.Repositories.Core.Video
{
    public class InStreamPositionRepository : RepositoryBase<InStreamPosition, int>, IInStreamPositionRepository
    {
        public InStreamPositionRepository(RepositoryImplBase<InStreamPosition, int> repository)
            : base(repository)
        {
        }

    }
}
