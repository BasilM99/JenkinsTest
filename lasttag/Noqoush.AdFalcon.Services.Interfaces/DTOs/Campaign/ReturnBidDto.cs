using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign
{
    [DataContract]
    public class ReturnBidDto
    {
        [DataMember]
        public IDictionary<int, decimal> CostModelsWrappersBidValues { get; set; }
      
    }
}
