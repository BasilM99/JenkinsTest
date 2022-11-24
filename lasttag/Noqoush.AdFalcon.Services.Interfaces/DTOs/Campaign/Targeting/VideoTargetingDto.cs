using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [DataContract]
   public  class VideoTargetingDto : TargetingBaseDto
    {
 

        #region Video Targeting
        [DataMember]
        public bool PlacementType_InStream { get; set; }
        [DataMember]
        public bool PlacementType_OutStream { get; set; }
        [DataMember]
        public bool PlacementType_Interstitial { get; set; }
        [DataMember]
        public bool PlacementType_Undetermined { get; set; }


        [DataMember]
        public bool InStreamPosition_PreRoll { get; set; }

        [DataMember]
        public bool InStreamPosition_MidRoll { get; set; }
        [DataMember]
        public bool InStreamPosition_PostRoll
        {
            get; set;
        }
        [DataMember]
        public bool InStreamPosition_Undetermined { get; set; }

        [DataMember]
        public bool SkippableAds_SkippableAdSpaces { get; set; }
        [DataMember]
        public bool SkippableAds_NonSkippableAdSpaces { get; set; }
        [DataMember]
        public bool SkippableAds_Undetermined { get; set; }
        [DataMember]
        public bool Playback_AutoPlaySoundOn { get; set; }
        [DataMember]
        public bool Playback_AutoPlaySoundOff { get; set; }
        [DataMember]
        public bool Playback_ClickToPlay { get; set; }
        [DataMember]
        public bool Playback_Undetermined { get; set; }
        [DataMember]
        public bool RewardedAds { get; set; }
        [DataMember]
        public bool RewardedAdOnly { get; set; }
        [DataMember]
        public bool MatchOrientation { get; set; }


        #endregion
    }
}
