using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account.Payment
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