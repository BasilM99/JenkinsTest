using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Fund;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Account.Fund
{
    public class FundTypeService : IFundTypeService
    {
        private readonly IAccountFundTypeRepository _accountFundTypeRepository = null;
        public FundTypeService(IAccountFundTypeRepository accountFundTypeRepository)
        {
            this._accountFundTypeRepository = accountFundTypeRepository;
        }

        public IEnumerable<AccountFundTypeDto> GetAll()
        {
            var paymentTypList = _accountFundTypeRepository.GetAll();
            var items = paymentTypList.Select(paymentTypeDto => MapperHelper.Map<AccountFundTypeDto>(paymentTypeDto)).ToList();
            return items;
        }
    }
}