using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Core
{
    public class SystemPayPalAccountService : ISystemPayPalAccountService
    {
        private readonly ISystemPayPalAccountRepository systemPayPalAccountRepository = null;
        public SystemPayPalAccountService(ISystemPayPalAccountRepository systemPayPalAccountRepository)
        {
            this.systemPayPalAccountRepository = systemPayPalAccountRepository;
        }

        public IEnumerable<Interfaces.DTOs.Core.SystemPayPalAccountDto> GetAll()
        {
            IEnumerable<SystemPayPalAccount> systemPayPalAccountList = systemPayPalAccountRepository.GetAll();
            var items =systemPayPalAccountList.Select(systemPayPalAccount => MapperHelper.Map<SystemPayPalAccountDto>(systemPayPalAccount)).ToList();
            return items;
        }
    }
}
