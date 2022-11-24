using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories
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
