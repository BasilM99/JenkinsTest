using System;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Campaign
{

    public class CreativeUnitCriteria
    {
        public DeviceTypeEnum DeviceType { get; set; }
        public AdTypeIds? AdType { get; set; }
        public AdSubTypes? AdSubType { get; set; }
        public string Group { get; set; }
        public override string ToString()
        {

            return base.ToString() + string.Format("{0}-{1}-{2}", DeviceType.ToString(), AdType.HasValue ? AdType.Value.ToString() : "-1", AdSubType.HasValue ? AdSubType.Value.ToString() : "-1");
        }
    }
}