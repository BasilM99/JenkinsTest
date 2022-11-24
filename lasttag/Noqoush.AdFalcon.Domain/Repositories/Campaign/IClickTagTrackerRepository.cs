using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
   

    public interface IClickTagTrackerRepository : IKeyedRepository<ClickTagTracker, int>
    {

    }

    public interface IThirdPartyTrackerRepository : IKeyedRepository<ThirdPartyTracker, int>
    {

    }

    
}
