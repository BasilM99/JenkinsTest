using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
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

    //[DataContract(Name = "VideoEndCardType")]
    //public enum VideoEndCardType
    //{
    //    [EnumMember]
     
    //    Static = 0,
    //    [EnumMember]
      
    //    Dynamic = 2
      

     
    //}
    public class VideoEndCard : IEntity<int>
    {
        public virtual int ID { get;  set; }
        public virtual Document Document { get; set; }
        public virtual string URL { get; set; }

        public virtual bool  AutoCloseOption { get; set; }
        public virtual double AutoCloseDuration { get; set; }
        public virtual string ClickURL { get; set; }

        public virtual VideoEndCardType Type { get; set; }
        public virtual CreativeUnit CreativeUnit { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual InStreamVideoCreative VideoAd { get; set; }


        public virtual VideoEndCard Clone()
        {
          
        VideoEndCard cloned = new VideoEndCard()
            {
                VideoAd = this.VideoAd,
                URL = this.URL,
                IsDeleted = this.IsDeleted,
                Document = this.Document,
                CreativeUnit = this.CreativeUnit,
                Type = this.Type,
                ClickURL = this.ClickURL,

            AutoCloseOption = this.AutoCloseOption,
            AutoCloseDuration = this.AutoCloseDuration
        };

            return cloned;
        }
        public virtual string GetDescription()
        {
            return this.URL;
        }

    }
}
