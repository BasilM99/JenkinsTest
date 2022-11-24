using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    //[DataContract()]
    //public enum BidConfigType
    //{
    //    [EnumMember]
    //    [EnumText("ActionType", "BidConfigType")]
    //    ActionType = 1,
    //    [EnumMember]
    //    [EnumText("Platform", "BidConfigType")]
    //    Platform = 2,
    //    [EnumMember]
    //    [EnumText("Manufacturer", "BidConfigType")]
    //    Manufacturer = 3,
    //    [EnumMember]
    //    [EnumText("Geographic", "BidConfigType")]
    //    Geographic = 4,
    //    [EnumMember]
    //    [EnumText("Operator", "BidConfigType")]
    //    Operator = 5,
    //    [EnumMember]
    //    [EnumText("Keyword", "BidConfigType")]
    //    Keyword = 6,
    //    [EnumMember]
    //    [EnumText("Demographic", "BidConfigType")]
    //    Demographic = 7,
    //    [EnumMember]
    //    [EnumText("Devices", "Global")]
    //    Devices = 9,
    //    [EnumMember]
    //    [EnumText("DeviceCapability", "BidConfigType")]
    //    DeviceCapability = 10,
    //    [EnumMember]
    //    [EnumText("AdType", "BidConfigType")]
    //    AdType = 11

    //}
    public class BidConfig
    {
        public virtual int Id { get; protected set; }
        public virtual BidConfigType Type { get; set; }
        public virtual int TargetingId { get; set; }
        public virtual AppScope AppScope { get; set; }
        public virtual int Value { get; set; }
        public virtual CostModelWrapper CostModelWrapper { get; set; }
        public virtual CostModelWrapperEnum CostModelWrapperEnum
        {
            get
            {
                if (this.CostModelWrapper != null)
                {
                    return (CostModelWrapperEnum)this.CostModelWrapper.ID;
                }

                return 0;
            }
        }
    }
}
