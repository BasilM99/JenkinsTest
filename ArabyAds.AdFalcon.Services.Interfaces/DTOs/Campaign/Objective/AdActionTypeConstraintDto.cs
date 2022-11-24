using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using System.Text;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective
{

    [Serializable]
    public enum DeviceConstraintDto
    {
        None,
        Tablet
    }
    [ProtoContract]
    public class AdActionTypeConstraintDto : LookupDto
    {
       [ProtoMember(1)]
        public PlatformDto Platform { get; set; }
       [ProtoMember(2)]
        public int DeviceConstraint { get; set; }
    }
}
