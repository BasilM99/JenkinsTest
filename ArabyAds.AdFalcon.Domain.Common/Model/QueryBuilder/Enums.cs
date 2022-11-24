using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Domain.Common.Model.QueryBuilder
{
    [Serializable]
    [DataContract()]
    public enum DataTypeQB
    {
        [EnumMember]
        Numeric = 0,
        [EnumMember]
        Bigint = 1,
        [EnumMember]
        Uuid = 2,
           [EnumMember]
        Hstore = 3,
        [EnumMember]
        Integer = 4,
        [EnumMember]
        String = 5,
        [EnumMember]
        Boolen = 6
    }

    [Serializable]
    [DataContract()]
    public enum Function
    {
        [EnumMember]
        CSV = 2,
        [EnumMember]
        Sample = 1,
        [EnumMember]
        Query = 0
    }

}
