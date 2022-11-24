
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Noqoush.AdFalcon.Domain.Common.Model.Core
{
    [DataContract()]
    public enum DeviceCapabilityType
    {
        [EnumMember]
        Both = 1,
        [EnumMember]
        Include = 2,
        [EnumMember]
        Exclude = 3
    }

    //public class DeviceCapability : ManagedLookupBase //LookupBase<DeviceCapability, int>
    //{
    //    public virtual string WurflCapabilities { get; set; }
    //    public virtual string WurflValue { get; set; }
    //    public virtual DeviceCapabilityType Type { get; set; }
    //}
}

