using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{
    [DataContract]
    [Serializable]
    public enum DeviceConstraintDto
    {
        [EnumMember]
        None,
        [EnumMember]
        Tablet
    }
    [DataContract]
    public class AdActionTypeConstraintDto : LookupDto
    {
        [DataMember]
        public PlatformDto Platform { get; set; }
        [DataMember]
        public int DeviceConstraint { get; set; }
    }
}
