using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Account.Payment;
using ArabyAds.AdFalcon.Domain.Repositories.Account.Payment;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Repositories
{
    class PaymentTypeRepository : RepositoryBase<PaymentType, int>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(RepositoryImplBase<PaymentType, int> repository)
            : base(repository)
        {
        }
    }
}
