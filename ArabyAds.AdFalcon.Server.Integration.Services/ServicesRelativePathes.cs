using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Server.Integration.Services
{
    static class ServicesRelativePathes
    {
        internal static class UserOpt 
        {
           public const string IsTrackingEnabled = "UserOpt/IsTrackingEnabled/{0}";
           public const string UpdateTracking = "UserOpt/UpdateTracking";

        }
        internal static class AudienceList 
        {
            public const string UpdateAudienceList = "AudienceList/UpdateAudienceList";
        }
    }
}
