using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports
{
    [DataContract()]
    public enum SummaryBy
    {
        [EnumMember]
        Hour = 0,
        [EnumMember]
        Day = 1,
        [EnumMember]
        Week = 2,
        [EnumMember]
        Month = 3,

        [EnumMember]
        Accumulated = 4
    }
}
