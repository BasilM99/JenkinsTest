﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.DomainServices.AuditTrial;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IAccountRepository : IKeyedRepository<Model.Account.Account, int>
    {
        string GetObjectName(int Id);
        IEnumerable<AuditTrialDto> GeAuditTrialMainRoots(AuditTrialFilter filter, out int TotalCount);
        bool IsRootObjectRelatedToAccount(int rootobjecid, int AccountId, int? UserId, string TypeName);
        IEnumerable<AuditTrialDto> GeAuditTrialForObjectRoot(AuditTrialFilter filter, out int TotalCount);
        string GetObjectNameForUserName(int Id);
        DateTime GeMaxActionTime(int objectRootId, int objectRootTypeId);
        IEnumerable<AuditTrialDto> GeAuditTrialMainRootsUsingStat(AuditTrialFilter filter, out int TotalCount);

        IEnumerable<AuditTrialDto> GeAuditTrialForObjectRootUsingStat(AuditTrialFilter filter, out int TotalCount);

        IEnumerable<Noqoush.AdFalcon.Domain.Model.Account.Account> QueryByCratiriaForUsers(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count);

        bool IsAccountDSP(int Id);
    }
}
