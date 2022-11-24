using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign.Creative
{
    public class AdRequestCriteria
    {
        public int AdGroupId { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.Creative.AdRequestCriteria Commoncr)
        {
            AdGroupId = Commoncr.AdGroupId;



            Page = Commoncr.Page;
            Size = Commoncr.Size;
        

        }

    }
}
