using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Core.Video
{
    [DataContract()]
    //public enum PlaybackMethodsEnum
    //{
    //    [EnumMember]
    //    AutoPlaywithSoundOn = 1,
    //    [EnumMember]
    //    AutoPlaywithSoundOff = 2,

    //    [EnumMember]
    //    ClicktoPlay = 3,
    //    [EnumMember]
    //    Undetermined = 0
    //}
    public class PlaybackMethods : ManagedLookupBase
    {
        public virtual string Code { get; set; }

    }
}
