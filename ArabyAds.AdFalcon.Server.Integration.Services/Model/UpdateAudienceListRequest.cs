using System;
using System.Collections.Generic;
using System.Text;

namespace ArabyAds.AdFalcon.Server.Integration.Services.Model
{
    public enum IDFASource
    {
        Android = 0,
        IOS = 1,
    }
    public class UpdateAudienceListRequest
    {
        public string IDFA { get; set; }
        public IDFASource IDFASource { get; set; }
        public int AudienceSegmentId { get; set; }
    }
}
