using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;

namespace ArabyAds.AdFalcon.Administration.Web.Controllers.Model.Lookup
{

    public class LookupViewModel
    {
        public virtual LookupDto LookupDto { get; set; }
        public string LookupType { get; set; }
        public string ViewName { get; set; }
        public string ActionName { get; set; }
    }

    public class LookupSaveModel
    {
        public string LookupType { get; set; }
        public string ViewName { get; set; }
        public string ActionName { get; set; }
    }
}
