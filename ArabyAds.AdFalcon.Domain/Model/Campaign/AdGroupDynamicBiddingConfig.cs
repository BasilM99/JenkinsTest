using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.Framework.DomainServices;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;

namespace ArabyAds.AdFalcon.Domain.Model.Campaign
{

    //[DataContract(Name = "BidOptimizationType")]
    //public enum BidOptimizationType
    //{
    //    [EnumMember]
    //    [EnumText("unkown", "Targeting")]
    //    Unknown = 0,
    //    [EnumMember]
    //    [EnumText("MaximizeCTR", "DynamicBidding")]
    //    MaximizeCTR = 1,

    //    [EnumMember]
    //    [EnumText("MinimizeCPC", "DynamicBidding")]
    //    MinimizeCPC = 2,
    //    [EnumMember]
    //    [EnumText("MinimizeCPA", "DynamicBidding")]
    //    MinimizeCPA = 3,

    //    [EnumMember]
    //    [EnumText("MinimizeeCPVCV", "DynamicBidding")]
    //    MinimizeeCPVCV = 4,

    //    [EnumMember]
    //    [EnumText("MaximizeVCVR", "DynamicBidding")]
    //    MaximizeVCVR = 5,
    //}
    public class AdGroupDynamicBiddingConfig : IEntity<int>
    {
      
        public virtual int ID { get; set; }
        public virtual AdGroup AdGroup { get; set; }
     
        public virtual BidOptimizationType Type
        {
            get;
            set;
        }

        public virtual decimal BidOptimizationValue { get; set; }
        public virtual decimal DefaultBidPrice { get; set; }
        public virtual decimal MaxBidPrice { get; set; }
        public virtual decimal MinBidPrice { get; set; }
        public virtual decimal BidStep { get; set; }
        public virtual bool KeepBiddingAtMinimum { get; set; }
        
        public virtual string GetDescription()
        {
            return Type.ToString();
        }
        public virtual AdGroupDynamicBiddingConfig Clone()
        {
            return new AdGroupDynamicBiddingConfig
            {
                Type = this.Type,
                BidOptimizationValue = this.BidOptimizationValue,
                DefaultBidPrice = this.DefaultBidPrice,
                MaxBidPrice = this.MaxBidPrice,
                MinBidPrice = this.MinBidPrice,

                BidStep = this.BidStep,
                KeepBiddingAtMinimum = this.KeepBiddingAtMinimum,
               
            };
        }

        public virtual bool IsDeleted { get; set; }

     
       
    }
}
