using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Core;


namespace Noqoush.AdFalcon.Domain.Common.Repositories.Core
{
    public class LookupCriteriaBase
    {
        public string LookType { get; set; }
     
    }
    public class LookupGetCriteria : LookupCriteriaBase
    {
        public int Id { get; set; }
      
    }

    [KnownType(typeof(DeviceLookupCriteria))]
    public class LookupCriteria : LookupGetCriteria
    {
        public string Name { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
 
    }

    public class DeviceLookupCriteria : LookupCriteria
    {
        public int? ManufacturerId { get; set; }
        public int? PlatformId { get; set; }

      
    }
}
