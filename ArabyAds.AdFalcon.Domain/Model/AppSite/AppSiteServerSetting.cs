using System.Runtime.Serialization;
using ArabyAds.Framework.DomainServices;
using ArabyAds.AdFalcon.Domain.Model.Core;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using System;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;

namespace ArabyAds.AdFalcon.Domain.Model.AppSite
{
    //[DataContract()]
    //public enum ImpressionCountMode
    //{
    //    [EnumMember]
    //    [EnumText("OnBeacon", "AppSite")]
    //    OnBeacon = 0,
    //    [EnumMember]
    //    [EnumText("OnBanner", "AppSite")]
    //    OnBanner = 1,
    //    [EnumMember]

    //    [EnumText("OnResponse", "AppSite")]
    //    OnResponse = 2
    //}

    //[DataContract()]
    //public enum AppSitePlacementType
    //{
    //    [EnumMember]
    //    [EnumText("DisplayPlacement", "Global")]
    //    Display = 0,
    //    [EnumMember]
    //    [EnumText("NativePlacement", "Global")]
    //    Native = 1,
    //    [EnumMember]

    //    [EnumText("RewardedPlacement", "Global")]
    //    Rewarded = 2
    //}
    public class AppSiteServerSetting : IEntity<int>
    {
        public AppSiteServerSetting()
        {
        }

        public AppSiteServerSetting(AppSite appSite)
        {
            AppSite = appSite;
        }

        public virtual int ID { get; protected set; }

        public virtual AppSite AppSite { get; protected set; }
        private int AppSiteId { get; set; }

        public virtual NativeAdLayout NativeAdLayout { get; set; }

        public virtual bool AllowBlindAds { get; set; }
        public virtual bool GenerateSystemUniqueId { get; set; }
        public virtual ImpressionCountMode ImpressionCountMode { get; set; }
        public virtual string SupportedAdTypes { get; set; }
        public virtual string SupportedBannerImageTypes { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual bool WatchTraffic { get; set; }
        public virtual int? AdRequestCacheLifeTime { get; set; }
        public virtual AppSitePlacementType AppSitePlacementType { get; set; }
        public virtual int? RewardedVideoItemValue { get; set; }
        public virtual string RewardedVideoItemName { get; set; }
        public virtual IList<AppSiteEvent> Events { get; set; }
        public virtual CostModelWrapper GetPricingModel()
        {
            AppSiteEvent appSiteEvent = this.Events.Where(x => x.IsBillable).FirstOrDefault();
            if (appSiteEvent != null)
            {
                return appSiteEvent.Event.CostModelWrapper;
            }
            return null;
        }


        public virtual bool IsUsingCampaignPricingModel
        {
            get
            {
                return this.GetPricingModel() == null;
            }
        }

        public virtual string GetDescription()
        {
            return string.Format("{0}:{1} {2}", AppSite.Name, AppSite.ID,Framework.Resources.ResourceManager.Instance.GetResource("ServerSettings", "AppSite"));
        }
        public virtual string getSupportedTypeDescription(string value)
        {
            string resultStr = string.Empty;

            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                return string.Format("{0}\n{1}\n", Framework.Resources.ResourceManager.Instance.GetResource("Banner", "Campaign"),  Framework.Resources.ResourceManager.Instance.GetResource("TextAd", "Campaign"));
         
                
            }
            else
            {
                var supportedAdTypes = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var IsSupportBannerAd = supportedAdTypes.Any(x => x == "1");
               var IsSupportTextAd = supportedAdTypes.Any(x => x =="2");
                if (IsSupportBannerAd)
                {
                    resultStr = Framework.Resources.ResourceManager.Instance.GetResource("Banner", "Campaign")+"\n";
                }

                if (IsSupportTextAd)
                {
                    resultStr += Framework.Resources.ResourceManager.Instance.GetResource("TextAd", "Campaign")+"\n";

                }

                return resultStr;
            }
           // return string.Empty;
        }
        public virtual IList<AppSiteEvent> EventsClone()
        {
            IList<AppSiteEvent> Events = new List<AppSiteEvent>();
            foreach (AppSiteEvent Event in this.Events)
            {
                Events.Add(Event);
            }

            return Events;
        }

        public virtual void ChangeNativeAdLayout(NativeAdLayout layout)
        {
            NativeAdLayout = layout;
        }
    }
}