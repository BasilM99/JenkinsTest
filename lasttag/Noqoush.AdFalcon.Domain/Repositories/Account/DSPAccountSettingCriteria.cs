﻿using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.Framework;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
{
   

    public class DSPAccountSettingCriteria : CriteriaBase<DSPAccountSetting>
    {

        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }


        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.DSPAccountSettingCriteria Commoncr)
        {









            Page = Commoncr.Page;

            Size = Commoncr.Size;
         
            Name = Commoncr.Name;

          



        }
        public override Expression<Func<DSPAccountSetting, bool>> GetExpression()
        {
            Expression<Func<DSPAccountSetting, bool>> filter = (c => (c.Account.ID == OperationContext.Current.UserInfo<IUserInfo>().AccountId.Value)



                                                           );

            return filter;
        }

        public override Func<DSPAccountSetting, bool> GetWhere()
        {
            Expression<Func<DSPAccountSetting, bool>> filter = GetExpression();
            return filter.Compile();


        }
    }
}
