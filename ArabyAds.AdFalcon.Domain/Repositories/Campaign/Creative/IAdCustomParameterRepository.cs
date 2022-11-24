﻿using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public interface IAdCustomParameterRepository : IKeyedRepository<AdCustomParameter, int>
    {
    }
}
