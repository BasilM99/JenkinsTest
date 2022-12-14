using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account.Payment;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account.Payment
{
    public class PaymentTypeMapping : ClassMap<PaymentType>
    {
        public PaymentTypeMapping()
        {
            Table("`account_payment_trans_type`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'PaymentType'");
            Map(x => x.IsDeleted);
            References(x => x.Name, "NameId");
            Cache.Transactional().ReadWrite().IncludeAll();
        }
    }
}