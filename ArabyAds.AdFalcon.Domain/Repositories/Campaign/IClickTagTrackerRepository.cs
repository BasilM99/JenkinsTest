using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Domain.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
   

    public interface IClickTagTrackerRepository : IKeyedRepository<ClickTagTracker, int>
    {

    }

    public interface IThirdPartyTrackerRepository : IKeyedRepository<ThirdPartyTracker, int>
    {

    }

    
}
