using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArabyAds.Framework.Persistence;
namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{
    

    public interface IAdGroupDynamicBiddingConfigRepository : IKeyedRepository<AdGroupDynamicBiddingConfig, int>
    {

    }
}
