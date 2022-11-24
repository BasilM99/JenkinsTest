using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class NativeAdIcon : NativeAdCreativeBase
    {
        public virtual NativeAdIcon Copy(NativeAdCreative nativeAd)
        {
            NativeAdIcon cloned = new NativeAdIcon()
            {
                AdCreative = nativeAd,
                URL = this.URL,
                IsDeleted = this.IsDeleted,
                Document = this.Document,
                CreativeUnit = this.CreativeUnit,
                MIMEType = this.MIMEType
            };

            return cloned;
        }
        public override string GetDescription()
        {
            return this.CreativeUnit.GetDescription();
        }
    }
}
