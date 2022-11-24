using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{
   
    public class HouseAdCriteria : CriteriaBase<HouseAd>
    {
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public bool IsPrimaryUser { get; set; }
        public int? Page { get; set; }
        public int Size { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }

        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.HouseAdCriteria Commoncr)
        {
            AccountId = Commoncr.AccountId;

            UserId = Commoncr.UserId;

            Page = Commoncr.Page;

            Size = Commoncr.Size;

            IsPrimaryUser = Commoncr.IsPrimaryUser;


            DataFrom = Commoncr.DataFrom;
            DataTo = Commoncr.DataTo;
        }

        public override Expression<Func<HouseAd, bool>> GetExpression()
        {
            Expression<Func<HouseAd, bool>> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId/* &&(!UserId.HasValue || c.User.ID==UserId)*/ );
            return filter;
        }

        public override Func<HouseAd, bool> GetWhere()
        {
            Func<HouseAd, bool> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId /*&&(!UserId.HasValue || c.User.ID==UserId)*/ );
            return filter;
        }
    }
}