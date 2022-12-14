using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using ArabyAds.AdFalcon.Domain.Model.Account.Discount;
using ArabyAds.AdFalcon.Domain.Model.Core;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class AccountDiscountMapping : ClassMap<AccountDiscount>
    {
        public AccountDiscountMapping()
        {
            Table("`account_discount`");
            Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
                                           MappingSettings._nextHi,
                                           MappingSettings._maxLo,
                                           "TableKey = 'AccountDiscount'");
            References(x => x.Account, "AccountId").Not.Nullable();
            Component(x => x.Discount, m =>
                                           {
                                               m.Map(x => x.Value, "Discount").Not.Nullable();
                                               m.Map(x => x.FromDate, "DiscountFromDate").Not.Nullable();
                                               m.Map(x => x.ToDate, "DiscountToDate");
                                           });
        }
    }
}