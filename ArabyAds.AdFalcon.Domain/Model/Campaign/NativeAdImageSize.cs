using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
    public class NativeAdImageSize : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual string Description { get; set; }
        public virtual short Width { get; set; }
        public virtual short Height { get; set; }
        public virtual short Code { get; set; }
        public virtual short Priority { get; set; }
        public virtual bool IsRequired { get; set; }

        public virtual bool IsDeleted
        {
            get;
            set;
        }
        public virtual string GetDescription()
        {
            return string.Format("{0}x{1}", this.Width, this.Height);
        }
    }

}
