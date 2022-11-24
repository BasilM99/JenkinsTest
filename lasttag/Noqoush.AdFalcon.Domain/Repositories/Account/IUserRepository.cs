using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public interface IUserRepository : IKeyedRepository<User, int>
    {
        IEnumerable<User> QueryByCratiriaForUsers(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count);
        IEnumerable<User> GetPublishedUsers(AllAppSiteCriteria criteria, out int Count);

        IEnumerable<User> GetSSPPartners(AllAppSiteCriteria criteria, out int Count);


       

        int GetUserAccountIdByEmail(string emailAddress);
    }
}
