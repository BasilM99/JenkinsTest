using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.DomainServices.AuditTrial;
namespace Noqoush.AdFalcon.Domain.Repositories.Account
{


    public interface IAccountInvitationRepository : IKeyedRepository<AccountInvitation, int>
    {
    }
    public interface IAccountDSPRequestRepository : IKeyedRepository<AccountDSPRequest, int>
    {
        bool CheckEmailAddress(string emailAddress);

        IEnumerable<AccountDSPRequest> QueryByCratiriaForAccountDSPRequests(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count);

        AccountDSPRequest GetByEmailAddress(string emailAddress);

        AccountDSPRequest GetByRequestCode(string requestCode);
        AccountDSPRequest GetByEmailAddressApproved(string emailAddress);
        bool CheckEmailAddressInvited(string emailAddress);

    }

    
}
