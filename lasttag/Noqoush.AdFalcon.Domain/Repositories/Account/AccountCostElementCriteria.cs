using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework;
using Noqoush.Framework.UserInfo;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
{
    public class AccountCostElementCriteria : CriteriaBase<AccountCostElement>
    {
        
             public int AccountId { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.AccountCostElementCriteria Commoncr)
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



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.AccountFeeCriteria Commoncr)
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
