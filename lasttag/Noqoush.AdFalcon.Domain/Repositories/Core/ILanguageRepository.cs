using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ILanguageRepository : IKeyedRepository<Language,int>
    {
    }
}
