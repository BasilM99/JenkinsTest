using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{

    public enum FrequencyCappingType
    {


        [EnumText("Evenly", "CampaignSettings")]
        Evenly = 1,
        [EnumText("FastMode", "CampaignSettings")]
        FastMode = 2
    }

    public enum FrequencyCappingInterval
    {
        [EnumText("Hour", "Global")]
        Hour = 3600,
        [EnumText("Day", "Global")]
        Day = 86400,

        [EnumText("Week", "Global")]
        Week = 604800,
        [EnumText("Month", "Global")]
        Month = 2592000,
        [EnumText("CampLifeTime", "Global")]
        LifeTime = 7776000


    }
  /*  public class CampaignFrequencyCapping : IEntity<int>
    {
        public virtual int ID { get; protected set; }

        public virtual bool IsDeleted { get; set; }

        public virtual TrackingEvent Event { get; set; }

        public virtual int Number { get; set; }
        public virtual int Interval { get; set; }
        public virtual int Type { get; set; }

        public virtual CampaignServerSetting CampaignServerSetting { get; set; }
        public virtual string GetIntervalDescription(string intervalVariable)
        {
            if (!string.IsNullOrEmpty(intervalVariable))
            {
                Enum enumTobe = (Enum)Enum.Parse(typeof(FrequencyCappingInterval), intervalVariable);
                if (Convert.ToInt32(intervalVariable) > 0)
                {
                    return enumTobe.ToText();
                }
                else
                {
                 return   FrequencyCappingType.FastMode.ToText();
                }
                //return string.Empty;

            }
            else
            {
                return string.Empty;

            }
        }

        public virtual string GetTypeDescription(string typeVariable)
        {
            if (!string.IsNullOrEmpty(typeVariable))
            {
                Enum enumTobe = (Enum)Enum.Parse(typeof(FrequencyCappingType), typeVariable);

                if (Convert.ToInt32(typeVariable) > 0)
                {
                    return enumTobe.ToText();
                }
                return string.Empty;
            }
            else
            {
                return string.Empty;

            }
        }
        public virtual string GetDescription()
        {
            return Event.GetDescription();
        }
    }*/
}
