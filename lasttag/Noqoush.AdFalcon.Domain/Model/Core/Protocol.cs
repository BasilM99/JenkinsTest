using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Core
{
    //[DataContract()]
    //public enum ProtocolCode
    //{
    //    [EnumMember]
    //    Undefined = 0,
     

    //    [EnumMember]
    //    VAST_2 =1,
    //    [EnumMember]
    //    VAST_3 = 2,
    //    [EnumMember]
    //    VAST_4 = 3,
    //    [EnumMember]
    //    VAST2_Wrapper = 4,
    //    [EnumMember]
    //    VAST3_Wrapper =5,
    //    [EnumMember]
    //    VAST4_Wrapper = 6,
    //    [EnumMember]
    //    VAST41 = 7,
    //    [EnumMember]
    //    VAST42 = 9,

    //}

    public class Protocol : ManagedLookupBase
    {
        public virtual int Code { get; set; }
    }
}
