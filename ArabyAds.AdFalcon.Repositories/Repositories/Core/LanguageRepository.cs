using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    public class LanguageRepository : RepositoryBase<Language, int> , ILanguageRepository
    {
        public LanguageRepository(RepositoryImplBase<Language, int> repository)
            : base(repository)
        {


        }
    }
}
