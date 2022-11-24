using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class NativeAdCreativeBase : IEntity<int>
    {
        public virtual int ID { get; protected set; }
        public virtual Document Document { get; set; }
        public virtual string URL { get; set; }
        public virtual MIMEType MIMEType { get; set; }
        public virtual CreativeUnit CreativeUnit { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual NativeAdCreative AdCreative { get; set; }
        public virtual string GetDescription()
        {
            return this.URL;
        }

    }
}
