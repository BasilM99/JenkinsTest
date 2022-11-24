using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using ArabyAds.AdFalcon.Domain.Common.Model.AppSite;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ProtoBuf;

namespace ArabyAds.AdFalcon.Domain.Common.Repositories.Core
{
    [ProtoContract]
    [ProtoInclude(100, typeof(LookupGetCriteria))]
    public class LookupCriteriaBase
    {
        [ProtoMember(1)]
        public string LookType { get; set; }
     
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(LookupCriteria))]
    public class LookupGetCriteria : LookupCriteriaBase
    {
        [ProtoMember(1)]
        public int Id { get; set; }
      
    }

    [ProtoContract]
    [KnownType(typeof(DeviceLookupCriteria))]
    [ProtoInclude(100, typeof(DeviceLookupCriteria))]
    public class LookupCriteria : LookupGetCriteria
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public int Page { get; set; }
        [ProtoMember(3)]
        public int Size { get; set; }
 
    }

    [ProtoContract]
    public class DeviceLookupCriteria : LookupCriteria
    {
        [ProtoMember(1)]
        public int? ManufacturerId { get; set; }
        [ProtoMember(2)]
        public int? PlatformId { get; set; }

      
    }
}
