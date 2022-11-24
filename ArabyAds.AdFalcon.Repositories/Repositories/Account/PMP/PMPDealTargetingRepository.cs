using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.AdFalcon.Domain.Repositories.Account.PMP;
using ArabyAds.AdFalcon.Domain.Model.Account.PMP;
namespace ArabyAds.AdFalcon.Persistence.Repositories.Account.PMP
{
   

    public class PMPDealTargetingRepository : RepositoryBase<PMPDealTargeting, int>, IPMPDealTargetingRepository
    {
        public PMPDealTargetingRepository(RepositoryImplBase<PMPDealTargeting, int> repository)
            : base(repository)
        {
        }


    }
}
