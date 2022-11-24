
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Noqoush.Framework;
using ControllerBase = Noqoush.AdFalcon.Web.Controllers.Core.ControllerBase;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using System.Collections.Generic;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Noqoush.AdFalcon.Common.UserInfo;
using System.Linq;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]

    public class AdvertiserController : ControllerBase
    {
        private IAdvertiserService _AdvertiserService;
        public AdvertiserController(IAdvertiserService AdvertiserService)
        {
            _AdvertiserService = AdvertiserService;
        }
        public ActionResult Index()
        {
            

            return View();
        }
        [RequireHttps]
        public ActionResult GetAdvertisersSecure(string q, string culture)
        {
            return Json(ReturnAdvertiserResult(q, culture), JsonRequestBehavior.AllowGet);
        }
        [RequireHttps]
        public ActionResult GetById(string ids)
        {
            IList<AdvertiserDto> results = new List<AdvertiserDto>();
            if (string.IsNullOrEmpty(ids))
                return Json(results, JsonRequestBehavior.AllowGet);
            List<int> TagIds = ids.Split(',').Select(int.Parse).ToList();
     
            foreach (var id in TagIds)
            {
                var item = _AdvertiserService.Get(id);
                    results.Add(item);
                    }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Duration = 9200, VaryByParam = "q;page")]
        [RequireHttps]
        public ActionResult GetAdvertisersSecurePagination(string q, string culture,int page)
        {
            return Json(ReturnAdvertiserResultWithPagination(q, culture,page), JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Duration = 3600, VaryByParam = "q;culture")]
        public ActionResult GetAdvertisers(string q, string culture)
        {
            
            return Json(ReturnAdvertiserResult(q, culture), JsonRequestBehavior.AllowGet);
        }


        private AdvertiserResult ReturnAdvertiserResultWithPagination(string q, string culture,int? page)
        {
            AdvertiserResult Advertisers = null;
         
            AdvertiserCriteria criteria = new AdvertiserCriteria() { Value = q, Culture = culture };
            criteria.Size = 10;
                 criteria.Page = page;

            criteria.Culture = Thread.CurrentThread.CurrentUICulture.Name;
            Advertisers = _AdvertiserService.GetByQueryPagination(criteria);
            return Advertisers;


        }

        private IEnumerable<AdvertiserDto> ReturnAdvertiserResult(string q, string culture)
        {
           

            AdvertiserCriteria criteria = new AdvertiserCriteria() { Value = q, Culture = culture };
         
              var  Advertisers = _AdvertiserService.GetByQuery(criteria);



            return Advertisers;
        }
      // [OutputCache(Duration = 3600, VaryByParam = "q;culture")]
        public ActionResult GetAccountAdvertisers(string q, string culture, int? page)
        {

            return Json(ReturnAccountAdvertiserResult(q, culture,  page), JsonRequestBehavior.AllowGet);
        }
        [RequireHttps]
        public ActionResult GetAccountAdvertisersForhttps(string q, string culture, int? page)
        {

            return Json(ReturnAccountAdvertiserResult(q, culture, page), JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Duration = 3600, VaryByParam = "userId")]
        public ActionResult GetAccountAdvertisersbyUser(int userId)
        {

            return Json(ReturnAccountAdvertiserResult(string.Empty, string.Empty,null), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<AdvertiserAccountListDto> ReturnAccountAdvertiserResult(string q, string culture, int? page)
        {
         

            var criteria = new AdvertiserAccountCriteria() { Name=q,culture= culture };
            criteria.AccountId = Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value;
            if (page.HasValue)
            { criteria.Page = page;
                criteria.Size = 10;

            }

            criteria.culture = Thread.CurrentThread.CurrentUICulture.Name;
            var IsPrimaryUser = OperationContext.Current.UserInfo<AdFalconUserInfo>().IsPrimaryUser;
            var UserId = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value;

            if (!IsPrimaryUser)
            {

                criteria.userId = UserId;

            }
            var Advertisers = _AdvertiserService.GetAccountAdvertiser(criteria);
            if (Advertisers != null)
                return Advertisers.Items;
            else
                return new List<AdvertiserAccountListDto>();
        }
    }
}
