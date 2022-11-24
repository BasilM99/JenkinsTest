using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Account.Fund
{
    public class FundTransTypeService : IFundTransTypeService
    {
        private readonly IAccountFundTransTypeRepository _accountFundTransTypeRepository = null;
        public FundTransTypeService(IAccountFundTransTypeRepository accountFundTransTypeRepository)
        {
            this._accountFundTransTypeRepository = accountFundTransTypeRepository;
        }

        public IEnumerable<AccountFundTransTypeDto> GetAll()
        {
            var paymentTypList = _accountFundTransTypeRepository.Query(x => x.AllowImpersonate);
            var items = paymentTypList.Select(paymentTypeDto => MapperHelper.Map<AccountFundTransTypeDto>(paymentTypeDto)).ToList();
            return items;
        }
    }
}
