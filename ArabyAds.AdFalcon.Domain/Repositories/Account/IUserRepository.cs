using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public interface IUserRepository : IKeyedRepository<User, int>
    {
        IEnumerable<User> QueryByCratiriaForUsers(Domain.Repositories.Account.UserCriteriaBase criteria, out int Count);
        IEnumerable<User> GetPublishedUsers(AllAppSiteCriteria criteria, out int Count);

        IEnumerable<User> GetSSPPartners(AllAppSiteCriteria criteria, out int Count);


       

        int GetUserAccountIdByEmail(string emailAddress);
    }
}
