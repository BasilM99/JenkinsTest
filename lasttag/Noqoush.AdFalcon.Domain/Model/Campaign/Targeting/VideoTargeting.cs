using Noqoush.AdFalcon.Domain.Model.Core.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
    public class VideoTargeting : TargetingBase
    {
        public override string GetDescription()
        {
            return string.Empty;
        }


        public virtual IList<PlacementType> PlacementTypes { get; set; }

        public virtual IList<PlaybackMethods> PlayBackMethods { get; set; }

        public virtual IList<InStreamPosition> InStreamPositions { get; set; }

        public virtual IList<SkippableAds> SkippableAds { get; set; }
        public virtual bool PlacementType_InStream { get; set; }
        public virtual bool PlacementType_OutStream { get; set; }
        public virtual bool PlacementType_Interstitial { get; set; }
        public virtual bool PlacementType_Undetermined { get; set; }



        public virtual bool InStreamPosition_PreRoll { get; set; }
        public virtual bool InStreamPosition_MidRoll { get; set; }
        public virtual bool InStreamPosition_PostRoll { get; set; }

        public virtual bool InStreamPosition_Undetermined { get; set; }


        public virtual bool SkippableAds_SkippableAdSpaces { get; set; }
        public virtual bool SkippableAds_NonSkippableAdSpaces { get; set; }

        public virtual bool SkippableAds_Undetermined { get; set; }

        public virtual bool Playback_AutoPlaySoundOn { get; set; }

        public virtual bool Playback_AutoPlaySoundOff { get; set; }
        public virtual bool Playback_ClickToPlay { get; set; }

        public virtual bool Playback_Undetermined { get; set; }
        public virtual bool RewardedAds { get; set; }

        public virtual bool RewardedAdOnly { get; set; }
        public virtual bool MatchOrientation { get; set; }

        public override TargetingBase Copy()
        {
            var cloneObj = new VideoTargeting()
            {

                AdGroup = this.AdGroup,
                Type = this.Type,
                IsDeleted = this.IsDeleted,
                PlacementType_InStream = this.PlacementType_InStream,
                PlacementType_OutStream = this.PlacementType_OutStream,
                PlacementType_Interstitial = this.PlacementType_Interstitial,
                PlacementType_Undetermined = this.PlacementType_Undetermined,
                InStreamPosition_PreRoll = this.InStreamPosition_PreRoll,
                InStreamPosition_MidRoll = this.InStreamPosition_MidRoll,
                InStreamPosition_PostRoll = this.InStreamPosition_PostRoll,
                InStreamPosition_Undetermined = this.InStreamPosition_Undetermined,
                SkippableAds_SkippableAdSpaces = this.SkippableAds_SkippableAdSpaces,
                SkippableAds_NonSkippableAdSpaces = this.SkippableAds_NonSkippableAdSpaces,
                SkippableAds_Undetermined = this.SkippableAds_Undetermined,
                Playback_AutoPlaySoundOn = this.Playback_AutoPlaySoundOn,
                Playback_AutoPlaySoundOff = this.Playback_AutoPlaySoundOff,
                Playback_ClickToPlay = this.Playback_ClickToPlay,
                Playback_Undetermined = this.Playback_Undetermined,
                RewardedAds = this.RewardedAds,
                RewardedAdOnly = this.RewardedAdOnly,
                MatchOrientation = this.MatchOrientation,
                PlacementTypes = new List<PlacementType>(this.PlacementTypes),
                PlayBackMethods = new List<PlaybackMethods>(this.PlayBackMethods),

                InStreamPositions = new List<InStreamPosition>(this.InStreamPositions),

                SkippableAds = new List<SkippableAds>(this.SkippableAds)

            };



            return cloneObj;
        }

    }
}
