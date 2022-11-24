using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Model.Lookup
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
