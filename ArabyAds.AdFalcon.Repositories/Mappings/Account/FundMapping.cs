using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    //Osaleh :- remove this class becuse it same as AccountFundTransHistory
    // public class FundMapping : ClassMap<Fund>
    //{
    //    public FundMapping()
    //    {
    //        Table("`account_fund_trans_history`");
    //        Id(x => x.ID).GeneratedBy.HiLo(MappingSettings.HiLowTableName,
    //                                       MappingSettings._nextHi,
    //                                       MappingSettings._maxLo,
    //                                       "TableKey = 'account_fund_trans_history'");
    //        OptimisticLock.Version();
    //        Map(x => x.Amount);
    //        Map(x => x.Currency);
    //        Map(x => x.Payee);
    //        Map(x => x.TransactionId);
    //        Map(x => x.FundDate).Column("TransactionDate");
    //        References(x => x.Account).Column("AccountId").Not.Nullable();
    //        References(x => x.User).Column("UserId").Not.Nullable();
    //        References(x => x.Type)
    //            .Class(typeof(FundType))
    //            .Not.Nullable()
    //            .Column("account_trans_type_Id")
    //            .Fetch.Select();
    //    }
    //}
}