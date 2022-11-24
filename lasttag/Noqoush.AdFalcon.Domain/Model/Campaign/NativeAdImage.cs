using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class NativeAdImage : NativeAdCreativeBase
    {
        public virtual NativeAdImage Copy(NativeAdCreative nativeAd)
        {
            NativeAdImage cloned = new NativeAdImage()
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
