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
    class PaymentRepository : RepositoryBase<Payment, int>, IPaymentRepository
    {
        public PaymentRepository(RepositoryImplBase<Payment, int> repository)
            : base(repository)
        {
        }
    }
}
