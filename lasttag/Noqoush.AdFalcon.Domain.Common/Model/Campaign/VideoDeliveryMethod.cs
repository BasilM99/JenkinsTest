using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract()]
    public enum VideoDeliveryMethodType
    {
        Streaming = 1,
        Progressive = 2
    }
  
}
