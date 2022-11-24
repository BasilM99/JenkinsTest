using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.Framework.Persistence;

namespace Noqoush.AdFalcon.Domain.Repositories
{
    public class AppSiteCriteria : CriteriaBase<AppSite>
    {
        public int AccountId { get; set; }
        public DateTime? DataFrom { get; set; }
        public DateTime? DataTo { get; set; }
        public int? Type { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public override Expression<Func<AppSite, bool>> GetExpression()
        {
            Expression<Func<AppSite, bool>> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId);
            if (Type.HasValue)
            {
                filter = c => c.IsDeleted == false && c.Account.ID == AccountId && (Type.Value == c.Type.ID);
            }
            return filter;
        }

        public override Func<AppSite, bool> GetWhere()
        {
            Func<AppSite, bool> filter = (c => c.IsDeleted == false && c.Account.ID == AccountId);
            if (Type.HasValue)
            {
                filter = c => c.IsDeleted == false && c.Account.ID == AccountId && (Type.Value == c.Type.ID);
            }
            return filter;
        }
    }
}
