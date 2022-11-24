﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public interface ImetriceColumnRepository : IKeyedRepository<metriceColumn, int>
    {
        int GetColumnId(string AppFieldName, bool Publisher);
    }
}