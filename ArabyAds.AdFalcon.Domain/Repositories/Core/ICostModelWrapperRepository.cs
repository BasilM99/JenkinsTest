using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface ICostModelWrapperRepository : IKeyedRepository<CostModelWrapper, int>
    {
    }
}
