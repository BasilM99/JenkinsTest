﻿using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Services
{
    public interface ITargeting
    {
        IList<AdRequestTargeting> GetAdRequests(int AdGroupId);
    }
}
