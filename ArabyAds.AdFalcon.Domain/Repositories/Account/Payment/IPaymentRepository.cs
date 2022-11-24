using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account.Payment
{
    public interface IPaymentRepository : IKeyedRepository<Model.Account.Payment.Payment, int>
    {
    }
}
