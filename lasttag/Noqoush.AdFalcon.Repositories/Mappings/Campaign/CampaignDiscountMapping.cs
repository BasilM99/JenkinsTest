using FluentNHibernate.Mapping;
using Noqoush.AdFalcon.Domain.Model.Account.Discount;

namespace Noqoush.AdFalcon.Persistence.Mappings.Campaign
{
    /*public class CampaignDiscountMapping : ClassMap<Campaign>
    {
        public CampaignDiscountMapping()
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
    }*/
}