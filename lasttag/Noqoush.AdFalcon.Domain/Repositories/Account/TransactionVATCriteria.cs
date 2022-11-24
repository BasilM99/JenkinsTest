using System;

using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.Persistence;
using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Account;

namespace Noqoush.AdFalcon.Domain.Repositories.Account
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



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Account.TransactionVATCriteria Commoncr)
        {




      

        DataTo = Commoncr.DataTo;

            DataFrom = Commoncr.DataFrom;

            IsPrimaryUser = Commoncr.IsPrimaryUser;

            UserId = Commoncr.UserId;
                        AccountId = Commoncr.AccountId;


        Page = Commoncr.Page;

            Size = Commoncr.Size;

            Details = Commoncr.Details;

            Payments = Commoncr.Payments;



        }
    }

}
