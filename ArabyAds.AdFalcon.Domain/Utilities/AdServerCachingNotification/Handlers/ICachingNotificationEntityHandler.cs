using ArabyAds.Framework.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArabyAds.AdFalcon.Domain.Utilities.AdServerCachingNotification.Handlers
{
    /// <summary>
    /// Get changed entites from EventArgsBase to be refreshed in AdServer cache.
    /// </summary>
    internal interface ICachingNotificationEntityHandler
    {
        /// <summary>
        /// Get changed entites from EventArgsBase to be refreshed in AdServer cache. 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<ChangedEntity> HandleEntities(EventArgsBase args);
    }
}
