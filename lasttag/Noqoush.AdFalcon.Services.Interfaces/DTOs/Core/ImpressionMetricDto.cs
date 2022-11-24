using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Domain.Common.Model.Core.CostElement;
using Noqoush.Framework.DataAnnotations;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Core
{
    [DataContract]
    public class ImpressionMetricDto : LookupDto
    {
        [DataMember]
        public List<MetricVendorDto> MetricVendors { get; set; }
    }
}
