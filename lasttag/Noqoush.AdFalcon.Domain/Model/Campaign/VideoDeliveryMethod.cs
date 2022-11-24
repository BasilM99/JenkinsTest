using Noqoush.AdFalcon.Domain.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.Domain.Model.Campaign { 
//{
//    [DataContract()]
//    public enum VideoDeliveryMethodType
//    {
//        Streaming = 1,
//        Progressive = 2
//    }
    public class VideoDeliveryMethod : LookupBase<AdType, int>
    {
        public virtual string Code { get; set; }
    }
}
