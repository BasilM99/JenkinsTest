using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative
{
  

    public class ImpressionMetricCriteria
    {
        public int AdGroupId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }



        public void CopyFromCommonToDomain(Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative.ImpressionMetricCriteria Commoncr)
        {
            AdGroupId = Commoncr.AdGroupId;



            Page = Commoncr.Page;
            Size = Commoncr.Size;


        }
    }
}
