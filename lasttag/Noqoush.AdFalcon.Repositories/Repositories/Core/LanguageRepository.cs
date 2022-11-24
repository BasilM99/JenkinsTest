using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    public class LanguageRepository : RepositoryBase<Language, int> , ILanguageRepository
    {
        public LanguageRepository(RepositoryImplBase<Language, int> repository)
            : base(repository)
        {


        }
    }
}
