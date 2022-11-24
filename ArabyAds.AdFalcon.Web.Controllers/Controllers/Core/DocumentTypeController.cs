using System.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers.Core
{
    public class DocumentTypeController : AuthorizedControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;
        public DocumentTypeController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }

        public JsonResult Index(string query)
        {
            var items = _documentTypeService.GetAll();
            var result = new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
    }
}


