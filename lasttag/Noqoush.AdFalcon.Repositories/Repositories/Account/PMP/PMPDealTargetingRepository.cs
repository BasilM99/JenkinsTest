using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Repositories.Account.PMP;
using Noqoush.AdFalcon.Domain.Model.Account.PMP;
namespace Noqoush.AdFalcon.Persistence.Repositories.Account.PMP
{
   

    public class PMPDealTargetingRepository : RepositoryBase<PMPDealTargeting, int>, IPMPDealTargetingRepository
    {
        public PMPDealTargetingRepository(RepositoryImplBase<PMPDealTargeting, int> repository)
            : base(repository)
        {
        }


    }
}
