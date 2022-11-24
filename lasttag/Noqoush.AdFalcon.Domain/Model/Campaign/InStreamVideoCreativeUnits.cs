using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class InStreamVideoCreativeUnit : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual MIMEType VideoType { get; set; }
        public virtual VideoDeliveryMethod DeliveryMethod { get; set; }
        public virtual AdCreativeUnit AdCreativeUnit { get; set; }

        public virtual CreativeUnit OriginalCreativeUnit { get; set; }
        public virtual int BitRate { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual Document ThumbnailDoc { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual void Delete()
        {
            this.IsDeleted = true;

        }
        public virtual string GetDescription()
        {
            return Width.ToString() + " x " + Height.ToString();
        }
       
        public virtual InStreamVideoCreativeUnit Clone(AdCreativeUnit adCreativeUnit)
        {
            var inStreamVideoCreativeUnitCloned = new InStreamVideoCreativeUnit();
            inStreamVideoCreativeUnitCloned.DeliveryMethod = this.DeliveryMethod;
            inStreamVideoCreativeUnitCloned.VideoType = this.VideoType;
            inStreamVideoCreativeUnitCloned.BitRate = this.BitRate;
            inStreamVideoCreativeUnitCloned.Width = this.Width;
            inStreamVideoCreativeUnitCloned.Height = this.Height;
            inStreamVideoCreativeUnitCloned.ThumbnailDoc = this.ThumbnailDoc;
            inStreamVideoCreativeUnitCloned.AdCreativeUnit = adCreativeUnit;
            inStreamVideoCreativeUnitCloned.OriginalCreativeUnit = this.OriginalCreativeUnit;

           
            return inStreamVideoCreativeUnitCloned;
        }

      
    }
}
