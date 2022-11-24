using System.Collections.Generic;
using System.Linq;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Payment;
using Noqoush.AdFalcon.Services.Mapping;

namespace Noqoush.AdFalcon.Services.Services.Account.PaymentType
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private IPaymentTypeRepository paymentTypeRepository = null;
        public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
        }

        public IEnumerable<PaymentTypeDto> GetAll()
        {
            var paymentTypList = paymentTypeRepository.GetAll();
            var items = paymentTypList.Select(paymentTypeDto => MapperHelper.Map<PaymentTypeDto>(paymentTypeDto)).ToList();
            return items;
        }
    }
}
