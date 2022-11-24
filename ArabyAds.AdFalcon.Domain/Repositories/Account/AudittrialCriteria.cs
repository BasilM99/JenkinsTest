using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.Framework.DomainServices.AuditTrial;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Account
{
    public class AuditTrialCriteria
    {
        public int? AccountId { get; set; }
        public int? UserId { get; set; }

        public bool IsPrimaryUser { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public int? Page { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }
        public int Type { get; set; }
        public int ObjectRootId { get; set; }




        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Account.AuditTrialCriteria Commoncr)
        {









            Page = Commoncr.Page;

            Size = Commoncr.Size;
            ObjectRootId = Commoncr.ObjectRootId;

            Type = Commoncr.Type;


            UserName = Commoncr.UserName;
            Name = Commoncr.Name;

            DataFrom = Commoncr.DataFrom;
            DataTo = Commoncr.DataTo;


            AccountId = Commoncr.AccountId;

            UserId = Commoncr.UserId;
            IsPrimaryUser = Commoncr.IsPrimaryUser;

                 

    }
        // public int ObjectRootId { get; set; }
    }
}
