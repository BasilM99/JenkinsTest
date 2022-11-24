using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Account.Payment
{
    public interface IPaymentRepository : IKeyedRepository<Model.Account.Payment.Payment, int>
    {
    }
}
