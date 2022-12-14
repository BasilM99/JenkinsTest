using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class PaymentTypeMapping : ClassMap<PaymentType>
    {
        public PaymentTypeMapping()
        {
            Table("`paymenttype`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'PaymentType'");
            Map(x => x.IsDeleted);
            References(x => x.Name, "NameId");
        }
    }
}