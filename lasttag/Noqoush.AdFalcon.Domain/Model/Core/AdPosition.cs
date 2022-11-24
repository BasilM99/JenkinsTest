using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Noqoush.AdFalcon.Domain.Model.Core
{

    [DataContract()]
    public enum AdPositionEnum
    {
        [EnumMember]
        AboveTheFold= 1,
        [EnumMember]
       BelowTheFold = 2,

        [EnumMember]
        Unknown = 3,
   
    }
    public class AdPosition : ManagedLookupBase
    {

        public virtual string Code { get; set; }
    }
}
