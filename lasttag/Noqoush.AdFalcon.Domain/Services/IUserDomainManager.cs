using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Domain.Services
{
    public interface IUserDomainManager
    {
        /// <summary>
        /// Get User Domain by emailAddress
        /// </summary>
        /// <param name="emailAddress">User email address</param>
        /// <returns></returns>
        User GetUserByEmail(string emailAddress, bool checkPendingEmail);


        /// <summary>
        /// Get All Users Domain by emailAddress
        /// </summary>
        /// <param name="emailAddress">User email address</param>
        /// <returns></returns>
        IList<User> GetManyUsersByEmail(string emailAddress, bool checkPendingEmail);
        

        /// <summary>
        /// Get All users information
        /// </summary>
        /// <returns>List of Users</returns>
        IEnumerable<Model.Account.User> GetAllUser();


        bool IsPrimaryUser(int accountId, int userId);
    }
}
