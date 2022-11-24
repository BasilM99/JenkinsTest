using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account
{
    public enum ObjectTypes
    {
        [EnumMember]
        All = 0,
        [EnumMember]
        ReportScheduler = 1,
        [EnumMember]
        AppSite = 2,
        [EnumMember]
        Campaign = 3,
        [EnumMember]
        Account = 4

    }

}
