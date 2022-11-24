using Microsoft.AspNetCore.Mvc;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Web.Controllers.Core;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Web.Controllers.Controllers.Core
{
    public class DocumentTypeController : AuthorizedControllerBase
    {
        private readonly IDocumentTypeService _documentTypeService;
        public DocumentTypeController()
        {
            _documentTypeService = IoC.Instance.Resolve<IDocumentTypeService>();
        }

        public JsonResult Index(string query)
        {
            var items = _documentTypeService.GetAll();
            var result = new JsonResult (  items);
            return result;
        }
    }
}


