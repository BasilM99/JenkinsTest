using Noqoush.AdFalcon.Domain.Model.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Noqoush.Framework.Persistence;
namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
    

    public interface IAdGroupDynamicBiddingConfigRepository : IKeyedRepository<AdGroupDynamicBiddingConfig, int>
    {

    }
}
