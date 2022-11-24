using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core.Video
{
    [DataContract()]
    public enum SkippableAdsEnum
    {
        [EnumMember]
        SkippableAdSpaces = 1,
        [EnumMember]
        NonSkippableAdSpaces = 2,


        [EnumMember]
        Undetermined = 0
    }
    //public class SkippableAds : ManagedLookupBase
    //{

    //    public virtual string Code { get; set; }
    //}
}
