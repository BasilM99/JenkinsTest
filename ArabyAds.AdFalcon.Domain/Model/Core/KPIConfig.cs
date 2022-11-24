using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Model.Core
{
 
    public class KPIConfig : ManagedLookupBase
    {

        

        public virtual bool ForAdvertiser { get; set; }
        public virtual bool ForDeals { get; set; }
        public virtual bool ForCampaign { get; set; }

        public virtual bool ForPublisher { get; set; }
        public virtual bool FoDataProvider { get; set; }
        public virtual string DataBaseField { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual string GrowIcon { get; set; }
        public virtual string GroupKey { get; set; }
        public virtual string Icon { get; set; }
        public virtual string DisplayFormat { get; set; }
        public virtual bool ForDataProvider { get; set; }

        //public virtual DeviceType DeviceType { get; set; }


        public override string ToString()
        {
            return string.Format("{0}X{1}", DataBaseField, GroupKey);
        }
    }
}
