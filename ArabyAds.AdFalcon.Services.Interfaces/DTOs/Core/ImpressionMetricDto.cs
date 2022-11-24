using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Domain.Common.Model.Core.CostElement;
using ArabyAds.Framework.DataAnnotations;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core
{
    [ProtoContract]
    public class ImpressionMetricDto : LookupDto
    {
       [ProtoMember(1)]
        public List<MetricVendorDto> MetricVendors { get; set; } = new List<MetricVendorDto>();
    }
}
