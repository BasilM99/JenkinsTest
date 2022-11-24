using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign
{

    public class CreativeFormatsCriteria : CriteriaBase<CreativeFormat>
    {
        public string Value { get; set; }
        public string Culture { get; set; }


        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.CreativeFormatsCriteria Commoncr)
        {
            Value = Commoncr.Value;

            Culture = Commoncr.Culture; 
        }
        public override Expression<Func<CreativeFormat, bool>> GetExpression()
        {
            Expression<Func<CreativeFormat, bool>> filter = c => true;

            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = c => c.Name.GetValue(Culture).StartsWith(Value);
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = c => c.Name.ToString().StartsWith(Value);
            }
            return filter;
        }

        public override Func<CreativeFormat, bool> GetWhere()
        {
            Func<CreativeFormat, bool> filter = c => true;
            if (!string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(Culture))
            {
                filter = c => c.Name.GetValue(Culture).StartsWith(Value);
            }
            else if (!string.IsNullOrWhiteSpace(Value))
            {
                filter = c => c.Name.ToString().StartsWith(Value);
            }
            return filter;
        }
    }
}
