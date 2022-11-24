using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using System.Threading.Tasks;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports
{
    public enum SummaryBy
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Accumulated = 4
    }
}
