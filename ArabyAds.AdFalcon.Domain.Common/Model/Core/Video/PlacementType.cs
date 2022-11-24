using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Core.Video
{
    [DataContract()]
    public enum PlacementTypeEnum
    {
        [EnumMember]
        InStream = 1,
        [EnumMember]
        OutStream = 2,

        [EnumMember]
        Interstitial = 3,
        [EnumMember]
        Undetermined = 0
    }
    //public class PlacementType : ManagedLookupBase
    //{

    //    public virtual string Code { get; set; }
    //}
}
