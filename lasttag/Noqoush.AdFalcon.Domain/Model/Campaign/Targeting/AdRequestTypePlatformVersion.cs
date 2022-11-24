using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Model.Campaign.Targeting
{
   
    public class AdRequestTypePlatformVersion : IEntity<int>
    {
        public virtual int ID { get; protected set; }


        public virtual string Version { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual AdRequestType AdRequestType { get; set; }
        public virtual AdRequestPlatform AdRequestPlatform { get; set; }
        public virtual string GetDescription()
        {
            return this.Version;
        }
    }
}
