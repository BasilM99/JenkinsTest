using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
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
