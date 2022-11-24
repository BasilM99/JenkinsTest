using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account.SSP;

namespace Noqoush.AdFalcon.Domain.Repositories.Account.SSP
{
    public interface IDealCampaignMappingRepository : IKeyedRepository<DealCampaignMapping, int>
    {
        IEnumerable<DealCampaignMapping> QueryByCratiriaForDealCampaignMapping(DealCampaignMappingCriteria criteria);
    }
}

