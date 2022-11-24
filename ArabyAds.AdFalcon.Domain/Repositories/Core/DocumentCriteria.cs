using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Core
{
    public class DocumentCriteria : CriteriaBase<Document>
    {
        public override Expression<Func<Document, bool>> GetExpression()
        {
            Expression<Func<Document, bool>> filter = c => true;
            return filter;
        }

        public override Func<Document, bool> GetWhere()
        {
            Func<Document, bool> filter = c => true;
            return filter;
        }
    }
}
