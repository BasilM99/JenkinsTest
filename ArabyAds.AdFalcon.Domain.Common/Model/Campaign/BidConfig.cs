using ArabyAds.AdFalcon.Domain.Common.Model.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Common.Model.Campaign
{
    [DataContract()]
    public enum BidConfigCalculationMode
    {
        [EnumMember]
        Minimum = 1,
        [EnumMember]
        Maximum = 2,
        [EnumMember]
        Avarage = 3
    }
    [DataContract()]
    public enum BidConfigType
    {
        [EnumMember]
        [EnumText("ActionType", "BidConfigType")]
        ActionType = 1,
        [EnumMember]
        [EnumText("Platform", "BidConfigType")]
        Platform = 2,
        [EnumMember]
        [EnumText("Manufacturer", "BidConfigType")]
        Manufacturer = 3,
        [EnumMember]
        [EnumText("Geographic", "BidConfigType")]
        Geographic = 4,
        [EnumMember]
        [EnumText("Operator", "BidConfigType")]
        Operator = 5,
        [EnumMember]
        [EnumText("Keyword", "BidConfigType")]
        Keyword = 6,
        [EnumMember]
        [EnumText("Demographic", "BidConfigType")]
        Demographic = 7,
        [EnumMember]
        [EnumText("Devices", "Global")]
        Devices = 9,
        [EnumMember]
        [EnumText("DeviceCapability", "BidConfigType")]
        DeviceCapability = 10,
        [EnumMember]
        [EnumText("AdType", "BidConfigType")]
        AdType = 11

    }


    [DataContract()]
    public enum DimentionType
    {
        [EnumMember]
        [EnumText("Choose", "Global")]
        Any = 0,
        
        [EnumMember]
        [EnumText("DimensionTypeTime", "Global")]
        Time = 1,
        [EnumMember]
        [EnumText("DimensionTypeCountry", "Global")]
        Country = 2,
        [EnumMember]
        [EnumText("DimensionTypeRegion", "Global")]
        Region = 3,
        [EnumMember]
        [EnumText("DimensionTypeOperator", "Global")]
        Operator = 4,
        [EnumMember]
        [EnumText("DimensionTypeGeofence", "Global")]
        Geofence = 5,
        [EnumMember]
        [EnumText("DimensionTypeDeviceManufacturer", "Global")]
        DeviceManufacturer = 6,
        [EnumMember]
        [EnumText("DimensionTypeDeviceModel", "Global")]
        DeviceModel = 7,
        [EnumMember]
        [EnumText("DimensionTypeDevicePlatform", "Global")]
        DevicePlatform = 8,
        [EnumMember]
        [EnumText("DimensionTypeDeviceType", "Global")]
        DeviceType = 9,
        [EnumMember]
        [EnumText("DimensionTypeCreativeUnit", "Global")]
        CreativeUnit = 10,
        [EnumMember]
        [EnumText("DimensionTypeKeyword", "Global")]
        Keyword = 11,
        [EnumMember]
        [EnumText("DimensionTypeEnvironmentType", "Global")]
        EnvironmentType = 12,

    }
    //public class BidConfig
    //{
    //    public virtual int Id { get; protected set; }
    //    public virtual BidConfigType Type { get; set; }
    //    public virtual int TargetingId { get; set; }
    //    public virtual AppScope AppScope { get; set; }
    //    public virtual int Value { get; set; }
    //    public virtual CostModelWrapper CostModelWrapper { get; set; }
    //    public virtual CostModelWrapperEnum CostModelWrapperEnum
    //    {
    //        get
    //        {
    //            if (this.CostModelWrapper != null)
    //            {
    //                return (CostModelWrapperEnum)this.CostModelWrapper.ID;
    //            }

    //            return 0;
    //        }
    //    }
    //}
}
