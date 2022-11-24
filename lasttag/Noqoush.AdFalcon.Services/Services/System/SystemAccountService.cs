using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.System;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.NSystem
{
    public class SystemAccountService : ISystemAccountService
    {

        private readonly IAccountPaymentDetailsRepository _accountPaymentDetailsRepository = null;
        public SystemAccountService(IAccountPaymentDetailsRepository accountPaymentDetailsRepository)
        {
            this._accountPaymentDetailsRepository = accountPaymentDetailsRepository;
        }

        public IList<PaymentDetailDto> GetSystemPaymentDetails(PayemntAccountType accountType)
        {
            var list = _accountPaymentDetailsRepository.Query(x => x.IsSystem && x.IsActive && x.AccountType == accountType);
            return list.Select(x => MapperHelper.Map<PaymentDetailDto>(x)).ToList();
        }
    }
}
