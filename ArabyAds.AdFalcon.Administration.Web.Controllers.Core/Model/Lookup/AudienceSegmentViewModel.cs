using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{
    public class AudienceSegmentViewModel : LookupViewModel
    {
        public AudienceSegmentDto AudienceSegment;
      
    }

    public class AudienceSegmentSaveModel : LookupSaveModel
    {
        public AudienceSegmentDto LookupDto { get; set; }
    }
}
