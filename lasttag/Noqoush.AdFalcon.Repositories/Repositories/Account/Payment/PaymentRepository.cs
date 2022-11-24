using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Repositories.Account.Payment;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Repositories
{
    class PaymentRepository : RepositoryBase<Payment, int>, IPaymentRepository
    {
        public PaymentRepository(RepositoryImplBase<Payment, int> repository)
            : base(repository)
        {
        }
    }
}
