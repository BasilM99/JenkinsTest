using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting
{
    [ProtoContract]
   public  class VideoTargetingDto : TargetingBaseDto
    {
 

        #region Video Targeting
       [ProtoMember(1)]
        public bool PlacementType_InStream { get; set; }
       [ProtoMember(2)]
        public bool PlacementType_OutStream { get; set; }
       [ProtoMember(3)]
        public bool PlacementType_Interstitial { get; set; }
       [ProtoMember(4)]
        public bool PlacementType_Undetermined { get; set; }


       [ProtoMember(5)]
        public bool InStreamPosition_PreRoll { get; set; }

       [ProtoMember(6)]
        public bool InStreamPosition_MidRoll { get; set; }
       [ProtoMember(7)]
        public bool InStreamPosition_PostRoll
        {
            get; set;
        }
       [ProtoMember(8)]
        public bool InStreamPosition_Undetermined { get; set; }

       [ProtoMember(9)]
        public bool SkippableAds_SkippableAdSpaces { get; set; }
       [ProtoMember(10)]
        public bool SkippableAds_NonSkippableAdSpaces { get; set; }
       [ProtoMember(11)]
        public bool SkippableAds_Undetermined { get; set; }
       [ProtoMember(12)]
        public bool Playback_AutoPlaySoundOn { get; set; }
       [ProtoMember(13)]
        public bool Playback_AutoPlaySoundOff { get; set; }
       [ProtoMember(14)]
        public bool Playback_ClickToPlay { get; set; }
       [ProtoMember(15)]
        public bool Playback_Undetermined { get; set; }
       [ProtoMember(16)]
        public bool RewardedAds { get; set; }
       [ProtoMember(17)]
        public bool RewardedAdOnly { get; set; }
       [ProtoMember(18)]
        public bool MatchOrientation { get; set; }


        #endregion
    }
}
