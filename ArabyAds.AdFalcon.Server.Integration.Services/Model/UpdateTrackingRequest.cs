using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Server.Integration.Services.Model
{
    public class UpdateTrackingRequest
    {
        public bool TrackEnabled { get; set; }
        public string UserId { get; set; }
    }
}
