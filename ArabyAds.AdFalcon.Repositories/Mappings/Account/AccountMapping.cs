using FluentNHibernate.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using FluentNHibernate;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;

namespace ArabyAds.AdFalcon.Persistence.Mappings.Account
{
    public class AccountMapping : SubclassMap<Domain.Model.Account.Account>
    {
        public AccountMapping()
        {
            Table("`account`");
            KeyColumn("Id");
            Map(x => x.DefaultRevenuePercentage).Nullable();
            References(x => x.PrimaryUser, "PrimaryUserId");
            HasOne(x => x.AccountSummary).Cascade.SaveUpdate();
            HasOne(x => x.APIAccess).Cascade.All();
            //HasOne(p => p.AccountSummary).PropertyRef(Reveal.Member<AccountSummary>("AccountId")).Cascade.All();
            HasMany(d => d.PaymentDetails).KeyColumn("AccountId").Cascade.All();
            HasMany(d => d.Discounts).KeyColumn("AccountId").Cascade.All();
            Map(x => x.UserAgreementVersion);
            Map(x => x.AllowAPIAccess);

            Map(x => x.AccountRole).CustomType(typeof(AccountRole)).Nullable(); 
            References(x => x.buyer, "BuyerId");
            
            Map(x => x.AccountBusinessId, "Account_Biz_Id");

          //  References(x => x.Parent, "ParentId");
            References(x => x.TaxRegistration, "TaxRegistrationId");

            Map(x => x.TaxNo);
            Map(X => X.AgencyCommission, "AgencyCommissionModel").CustomType(typeof(AgencyCommission)).Nullable();
            Map(X => X.AgencyCommissionValue, "AgencyCommissionModelValue");
          
     


        }

    }
}