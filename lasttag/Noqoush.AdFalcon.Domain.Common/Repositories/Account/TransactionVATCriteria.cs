using System;

using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Account;

namespace Noqoush.AdFalcon.Domain.Common.Repositories.Account
{
   

    public class TransactionVATCriteria 
    {
        public int? AccountId { get; set; }
        public int? UserId { get; set; }

        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public bool Details { get; set; }
        public bool Payments { get; set; }
    }

}
