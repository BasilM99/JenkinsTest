using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ImetriceColumnRepository : IKeyedRepository<metriceColumn, int>
    {
        int GetColumnId(string AppFieldName, bool Publisher);
    }
}
