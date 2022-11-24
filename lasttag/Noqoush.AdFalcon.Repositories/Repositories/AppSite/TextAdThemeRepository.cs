using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    public class TextAdThemeRepository : RepositoryBase<TextAdTheme, int>, ITextAdThemeRepository
    {
        public TextAdThemeRepository(RepositoryImplBase<TextAdTheme, int> repository) : base(repository)
        {
        }
    }
}
