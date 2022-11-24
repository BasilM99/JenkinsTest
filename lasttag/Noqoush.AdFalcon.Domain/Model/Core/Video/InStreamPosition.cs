using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Core.Video
{

    //[DataContract()]
    //public enum InStreamPositionEnum
    //{
    //    [EnumMember]
    //    PreRoll = 1,
    //    [EnumMember]
    //    MidRoll = 2,

    //    [EnumMember]
    //   PostRoll = 3,
    //    [EnumMember]
    //   Undetermined = 0
    //}
    public class InStreamPosition : ManagedLookupBase
    {

        public virtual string Code { get; set; }
       

    }
}
