using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories
{
    public class KeywordCriteria : CriteriaBase<Keyword>
    {
        public string Value { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.KeywordCriteria Commoncr)
        {
            Value = Commoncr.Value;
          






        }
        public override Expression<Func<Keyword, bool>> GetExpression()
        {
            Expression<Func<Keyword, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = c => c.Name.ToString().StartsWith(Value);
            }
            return filter;
        }

        public override Func<Keyword, bool> GetWhere()
        {
            Func<Keyword, bool> filter = c => true;
            if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = c => c.Name.ToString().StartsWith(Value);
            }
            return filter;
        }
    }
}
