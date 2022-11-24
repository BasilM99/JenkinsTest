using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{

    public class CreativeVendorCriteria : CriteriaBase<CreativeVendor>
    {
        public string Value { get; set; }
        public string Culture { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CreativeVendorCriteria Commoncr)
        {
            Value = Commoncr.Value;

            Culture = Commoncr.Culture;

           
        }
        public override Expression<Func<CreativeVendor, bool>> GetExpression()
        {
            Expression<Func<CreativeVendor, bool>> filter = c => true;

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

        public override Func<CreativeVendor, bool> GetWhere()
        {
            Func<CreativeVendor, bool> filter = c => true;
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
