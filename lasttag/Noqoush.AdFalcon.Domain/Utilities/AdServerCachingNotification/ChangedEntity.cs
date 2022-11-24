using Noqoush.AdFalcon.EventDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noqoush.AdFalcon.Domain.Utilities.AdServerCachingNotification
{
    public class ChangedEntity
    {
        public EntityType EntityType { get; set; }

        public Dictionary<string, object> DirtyProperties { get; set; }

        public ChangedEntity()
        {

        }

        public ChangedEntity(EntityType entityType)
        {
            EntityType = entityType;
        }
    }
}
