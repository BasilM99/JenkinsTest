using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{
  

    public class VideoConversionCreativeUnit : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }
        public virtual int ID { get; set; }

        public virtual int AudioBitRate { get; set; }
        public virtual int VideoFrameRate { get; set; }
        public virtual string Code { get; set; }
        public virtual int BitRate { get; set; }
        public virtual CreativeUnit CreativeUnit { get;set;}
      
        public virtual string GetDescription()
        {
            return Code;

        }
    }
}
