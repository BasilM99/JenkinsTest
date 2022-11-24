using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Repositories;

namespace ArabyAds.AdFalcon.Domain.Services
{
    public class UserDomainManager : IUserDomainManager
    {
        private IUserRepository _userRepository = null;
        private IAccountRepository _accountRepository = null;
        public UserDomainManager(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            this._userRepository = userRepository;
            this._accountRepository = accountRepository;
        }
        public Model.Account.User GetUserByEmail(string emailAddress, bool checkPendingEmail)
        {
            if (checkPendingEmail)
                return _userRepository.Query(p => p.EmailAddress == emailAddress || p.PendingEmailAddress == emailAddress).SingleOrDefault();
            else
                return _userRepository.Query(p => p.EmailAddress == emailAddress).SingleOrDefault();
        }
        public IList<Model.Account.User> GetManyUsersByEmail(string emailAddress, bool checkPendingEmail)
        {
            if (checkPendingEmail)
                return _userRepository.Query(p => p.EmailAddress == emailAddress || p.PendingEmailAddress == emailAddress).ToList();
            else
                return _userRepository.Query(p => p.EmailAddress == emailAddress).ToList();
        }
        public bool IsPrimaryUser(int accountId,int userId)
        {
         
                var account= _accountRepository.Query(p => p.ID == accountId).SingleOrDefault();
            return account.PrimaryUser.ID == userId;
            
        }
        public IEnumerable<Model.Account.User> GetAllUser()
        {
            return _userRepository.GetAll();
        }
    }
}
