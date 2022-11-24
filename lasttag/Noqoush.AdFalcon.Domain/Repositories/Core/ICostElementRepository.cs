using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Core.CostElement;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Core;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ICostElementRepository : IKeyedRepository<CostElement, int>
    {
    }

    public interface IFeeRepository : IKeyedRepository<Fee, int>
    {
    }
}
