using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{


    public class VideoMediaFile : IEntity<int>
    {


        public virtual MIMEType VideoType { get; set; }
        public virtual int ID { get; set; }
        public virtual AdCreativeUnit AdCreativeUnit { get; set; }
        public virtual VideoDeliveryMethod DeliveryMethod { get; set; }
        public virtual string URL { get; set; }
        public virtual InStreamVideoCreative VideoAd { get; set; }
        public virtual CreativeUnit OriginalCreativeUnit { get; set; }
        public virtual int BitRate { get; set; }
        //public virtual int Width { get; set; }
        //public virtual int Height { get; set; }
        public virtual Document Document { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual void Delete()
        {
            this.IsDeleted = true;

        }



        public virtual string GetDescription()
        {
            return Document != null ? Document.GetDescription() : "";
        }

        public virtual VideoMediaFile Clone()
        {
            var inStreamVideoCreativeUnitCloned = new VideoMediaFile();
            inStreamVideoCreativeUnitCloned.DeliveryMethod = this.DeliveryMethod;
            inStreamVideoCreativeUnitCloned.VideoType = this.VideoType;
            inStreamVideoCreativeUnitCloned.BitRate = this.BitRate;
            inStreamVideoCreativeUnitCloned.VideoAd = this.VideoAd;

            inStreamVideoCreativeUnitCloned.AdCreativeUnit = this.AdCreativeUnit;
            inStreamVideoCreativeUnitCloned.OriginalCreativeUnit = this.OriginalCreativeUnit;
            inStreamVideoCreativeUnitCloned.Document = this.Document;

            inStreamVideoCreativeUnitCloned.URL = this.URL;
            return inStreamVideoCreativeUnitCloned;
        }

    }
}
