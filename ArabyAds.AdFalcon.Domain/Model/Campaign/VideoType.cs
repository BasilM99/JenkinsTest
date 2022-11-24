using ArabyAds.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class VideoType : LookupBase<AdType, int>
    {
        public virtual string Code { get; set; }
    }
}
