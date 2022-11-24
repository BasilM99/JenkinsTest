using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Account.Payment
{
    public interface IPaymentTypeRepository : IKeyedRepository<PaymentType, int>
    {

    }
}
