using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ILookupRepository : IKeyedRepository<ManagedLookupBase, int>
    //public interface ILookupRepository<T,I> : IKeyedRepository<LookupBase<T,I>, int>
    {
    }
}
