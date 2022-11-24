using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services
{
    public class SystemBankAccountService : ISystemBankAccountService
    {
        private ISystemBankAccountRepository systemBankAccountRepository = null;
        public SystemBankAccountService(ISystemBankAccountRepository systemBankAccountRepository)
        {
            this.systemBankAccountRepository = systemBankAccountRepository;
        }

        public IEnumerable<Interfaces.DTOs.Core.SystemBankAccountDto> GetAll()
        {
            IEnumerable<SystemBankAccount> systemBankAccountList = systemBankAccountRepository.GetAll();
            var items =systemBankAccountList.Select(systemBankAccount => MapperHelper.Map<SystemBankAccountDto>(systemBankAccount)).ToList();
            return items;
        }
    }
}
