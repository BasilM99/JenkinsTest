using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Persistence.Mappings.Account
{
    public class PaymentMapping : ClassMap<Payment>
    {
        public PaymentMapping()
        {
            Table("`payments`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'Payment'");
            Map(x => x.Amount);
            Map(x => x.PaymentDate);
            Map(x => x.ReceiptNo);
            Map(x => x.Comment);
            Map(x => x.AdFalconReceiptNo);
            Map(x => x.CreationDate);
            References(x => x.Attachment)
               .Column("AttachmentId")
               .Fetch.Select();
            References(x => x.Account)
                .Class(typeof(Domain.Model.Account.Account))
                .Not.Nullable()
                .Column("AccountId")
                .Fetch.Select();
            References(x => x.Type)
                .Class(typeof(PaymentType))
                .Not.Nullable()
                .Column("PaymentTypeId")
                .Fetch.Select();
            References(x => x.User)
                .Class(typeof(User))
                .Not.Nullable()
                .Column("UserId")
                .Fetch.Select();
        }
    }
}