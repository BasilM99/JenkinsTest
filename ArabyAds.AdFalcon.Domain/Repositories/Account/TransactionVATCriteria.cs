using System;

using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;
using System.Collections;
using System.Collections.Generic;

using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.Account;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
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



        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.TransactionVATCriteria Commoncr)
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
