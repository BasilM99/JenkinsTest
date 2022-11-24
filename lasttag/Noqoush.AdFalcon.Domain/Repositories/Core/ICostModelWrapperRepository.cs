﻿using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Core
{
    public interface ICostModelWrapperRepository : IKeyedRepository<CostModelWrapper, int>
    {
    }
}
