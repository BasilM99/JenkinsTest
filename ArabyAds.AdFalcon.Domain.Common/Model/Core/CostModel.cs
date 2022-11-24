using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Core
{
    [DataContract()]
    public enum CostModelEnum
    {
        [EnumMember]
        CPC = 1,
        [EnumMember]
        CPM = 2,
    }

    //public class CostModel : ManagedLookupBase
    //{

    //}
}
