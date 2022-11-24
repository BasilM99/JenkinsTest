using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework;
using ArabyAds.Framework.UserInfo;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
{
    public class AccountCostElementCriteria : CriteriaBase<AccountCostElement>
    {
        
             public int AccountId { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountCostElementCriteria Commoncr)
        {






           


            Page = Commoncr.Page;

            Size = Commoncr.Size;
            Name = Commoncr.Name;

            AccountId = Commoncr.AccountId;





        }



        public override Expression<Func<AccountCostElement, bool>> GetExpression()
        {
            Expression<Func<AccountCostElement, bool>> filter =(c =>  (c.Account.ID == OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value)
                                                          
                                                        
                                                           
                                                           );

            return filter;
        }

        public override Func<AccountCostElement, bool> GetWhere()
        {
            Expression<Func<AccountCostElement, bool>> filter = GetExpression();
            return filter.Compile();
           
          
        }
    }

    public class AccountFeeCriteria : CriteriaBase<AccountFee>
    {



        public int AccountId{ get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AccountFeeCriteria Commoncr)
        {









            Page = Commoncr.Page;

            Size = Commoncr.Size;
            Name = Commoncr.Name;

            AccountId = Commoncr.AccountId;





        }
        public override Expression<Func<AccountFee, bool>> GetExpression()
        {
            Expression<Func<AccountFee, bool>> filter = (c => (c.Account.ID == OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value)



                                                           );

            return filter;
        }

        public override Func<AccountFee, bool> GetWhere()
        {
            Expression<Func<AccountFee, bool>> filter = GetExpression();
            return filter.Compile();


        }
    }
}
