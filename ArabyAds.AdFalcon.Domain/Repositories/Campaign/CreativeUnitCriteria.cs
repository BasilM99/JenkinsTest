using System;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.Persistence;

namespace ArabyAds.AdFalcon.Domain.Repositories.Campaign
{

    public class CreativeUnitCriteria
    {
        public DeviceTypeEnum DeviceType { get; set; }
        public AdTypeIds? AdType { get; set; }
        public AdSubTypes? AdSubType { get; set; }
        public string Group { get; set; }


        public void CopyFromCommonToDomain(ArabyAds.AdFalcon.Domain.Common.Repositories.Campaign.CreativeUnitCriteria Commoncr)
        {
            Group = Commoncr.Group;

            AdSubType = Commoncr.AdSubType;

            AdType = Commoncr.AdType;

            DeviceType = Commoncr.DeviceType;
        }
        public override string ToString()
        {

            return base.ToString() + string.Format("{0}-{1}-{2}", DeviceType.ToString(), AdType.HasValue ? AdType.Value.ToString() : "-1", AdSubType.HasValue ? AdSubType.Value.ToString() : "-1");
        }
    }
}