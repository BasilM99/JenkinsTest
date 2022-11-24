using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    public class KeyWordRepository : RepositoryBase<Keyword, int>, IKeyWordRepository
    {
        public KeyWordRepository(RepositoryImplBase<Keyword, int> repository)
            : base(repository)
        {

        }

        public IEnumerable<Keyword> GetTop(int count)
        {
            return RepositoryImpl.EntitySet.OrderByDescending(item => item.Usage).Take(count);
        }
    }
}
