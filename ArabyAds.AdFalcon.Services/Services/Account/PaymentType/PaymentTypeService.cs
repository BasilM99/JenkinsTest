using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.Payment;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Payment;
using ArabyAds.AdFalcon.Services.Mapping;

namespace ArabyAds.AdFalcon.Services.Services.Account.PaymentType
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
