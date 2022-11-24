using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account
{
    public class AccountCostElementCriteria 
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        
    }

    public class AccountFeeCriteria 
    {

        public string Name { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public int AccountId { get; set; }

    }
}
