using Noqoush.AdFalcon.Domain.Common.Model.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Account.DPP
{

    [DataContract(Name = "ImpressionLogType")]
    public enum ImpressionLogType
    {
        [EnumMember]

        None = 0,
        [EnumMember]

        Impression = 1,
        [EnumMember]

        AdMarkup = 2,
    
    }
   
}
