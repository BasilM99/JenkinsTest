using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    
    public interface IAdRequestTypeRepository : IKeyedRepository<AdRequestType, int>
    {

    }
}
