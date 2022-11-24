using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;

using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Objective;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Web.Controllers.Model.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Tree;
using Noqoush.Framework;
using Noqoush.Framework.Utilities;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc.Extensions;
using Action = Noqoush.AdFalcon.Web.Controllers.Model.Action;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using System.Globalization;
using System.Text.RegularExpressions;
using Noqoush.AdFalcon.Services.Interfaces.Services.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Handler;
using Telerik.Web.Mvc.UI;
using System.Web;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account.PMP;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.AppSite;
using Noqoush.AdFalcon.Web.Controllers.Model.PMPDeal;
using Noqoush.AdFalcon.Domain.Common.Repositories;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;
using Noqoush.AdFalcon.Web.Controllers.Model.Advertiser;
using Noqoush.AdFalcon.Domain.Common.Repositories.Account;
using Noqoush.AdFalcon.Exceptions;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Web.Controllers.Model.AppSite.Performance;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Web.Controllers.Model.Audiance;
namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{

    //[DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider }, Roles = "AppOps", AuthorizeRoles = "Administrator,AccountManager,AdOps", DenyImpersonationOnly = true)]
    public class DataProviderController : AuthorizedControllerBase
    {



        private ITileImageService _tileImageService;
        private IDocumentService _DocumentService;
        protected ICostModelWrapperService _CostModelWrapperService;
        protected IPartyService partyService;
        protected ILookupService lookupService;
        protected IAudienceSegmentService audienceSegmentService;
        protected ICampaignService campaignService;
        public DataProviderController(IPartyService partyService, ILookupService lookupService, ITileImageService tileImageService, IDocumentService _DocumentService, ICostModelWrapperService costModelWrapperService, IAudienceSegmentService AudienceSegmentService, ICampaignService campServ) 
        {
            _CostModelWrapperService = costModelWrapperService;

            _tileImageService = tileImageService;
            this._DocumentService = _DocumentService;
            this.audienceSegmentService = AudienceSegmentService;
            this.partyService = partyService;
            this.lookupService = lookupService;
            this.campaignService = campServ;
        }
        #region Index
        #region Helpers
        private List<SelectListItem> GetList(string lookupType, int? selectedValue)
        {
            var items = lookupService.GetAllLookup(new LookupCriteriaBase { LookType = lookupType });
            var lookupsList = new List<SelectListItem> { new SelectListItem
                                              {
                                                  Value = "",
                                                  Text = ResourcesUtilities.GetResource("Select","Global"),
                                                  Selected = selectedValue==0
                                              }};
            lookupsList.AddRange(
                items.Items.Select(
                    item => new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name.ToString(),
                        Selected = item.ID == selectedValue
                    }));
            return lookupsList;
        }

        #endregion

       
        public ActionResult DataProviders()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Providers", "Audience"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            var model = LoadData(null, "DataProviders");

            model.BusinessPartnerTypes = GetList(LookupNames.BusinessPartnerType, null);
            return View("DataProviders", model);
        }
        [GridAction(EnableCustomBinding = true)]
       
        public ActionResult _DataProviders()
        {
            var result = GetQueryResult(null, "DataProviders");
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [HttpGet]
        public ActionResult GetExternalDataProviderById(int Id, int adgroupId , int AdvAccountId)
        {
          var DPProvider=  partyService.GetExternalDPPartner(Id);

           var stringList=  this.campaignService.getAudiancesUsedInIntegration(adgroupId , AdvAccountId, Id);
            var count = this.campaignService.getCountAudiancesUsedInIntegration(adgroupId, AdvAccountId, Id);
            var Databid = this.campaignService.getDataBidAudiancesUsedInIntegration(adgroupId, AdvAccountId, Id);
            var stringListAc = this.campaignService.getAudiancesUsedInIntegrationActive(adgroupId, AdvAccountId, Id);

            return  Content(DPProvider.SiteProviderURL+";"+ stringList+";"+count + ";" + Databid+";"+ stringListAc);
        }


        public ActionResult LandingDataProvider(int Id, string Source)
        {
            
            var DPProvider = partyService.GetExternalDPPartner(Id);
            Model.Audiance.DataProvider item = new Model.Audiance.DataProvider();

          var Lists=  partyService.QueryByCriteriaForDPPartner( new DPPartnerCriteria());

            if (Lists==null || Lists.Items ==null || Lists.Items.Where(M=>M.ID== DPProvider.ID).SingleOrDefault()==null )
            {
                throw new UnauthorizedAccessException();
            }
            //OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.HasValue;


            item.Id = DPProvider.ID.Value;
            item.Name = DPProvider.Name;
            item.SitePoviderURL = DPProvider.SiteProviderURL;
            item.BaseSitePoviderURL = DPProvider.SiteProviderURL;
        
            var textItem = string.Empty;
            if (Source == "Forgetpassword")
            {


                textItem = ResourcesUtilities.GetResource("Forgetpassword", "Titles");

                item.TitlePageHeader = textItem;
                item.SitePoviderURL = item.SitePoviderURL + "/" + "forgot-password";
            }
            else if (Source == "ChangePassword")
            {

                textItem = ResourcesUtilities.GetResource("ChangePassword", "Titles");

                item.SitePoviderURL = item.SitePoviderURL + "/" + "change-password";
                item.TitlePageHeader = textItem;
            }
            else if (Source == "Register")
            {

                textItem = ResourcesUtilities.GetResource("Register", "UserInformation");

                item.SitePoviderURL = item.SitePoviderURL + "/" + "register";
                item.TitlePageHeader = textItem;
            }
            else if (Source == "Reset")
            {

                textItem = "Reset Password";

                item.SitePoviderURL = item.SitePoviderURL + "/" + "reset-password";
                item.TitlePageHeader = textItem;
            }
           
            else if (Source == "Login")
            {

                textItem = ResourcesUtilities.GetResource("Login", "Titles");
                item.SitePoviderURL = item.SitePoviderURL + "/" + "login";
                item.TitlePageHeader = textItem;
            }
            else if (Source == "Audiance")
            {
                textItem = ResourcesUtilities.GetResource("AudienceLists", "Global");
                item.SitePoviderURL = item.SitePoviderURL + "/" + "audience-list";

                item.TitlePageHeader = textItem;

            }
            else
            {
                textItem = ResourcesUtilities.GetResource("AudienceLists", "Global");
                item.SitePoviderURL = item.SitePoviderURL + "/" + "audience-list";

                item.TitlePageHeader = textItem;
            }
            #region BreadCrumb
            var breadCrumbLinks = new List<BreadCrumbModel>();
            breadCrumbLinks = new List<BreadCrumbModel>()
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("Providers", "Audience"),
                                                  Order = 1,
                                                   //Url = Url.Action("DataProviders")
                                              }
                                             ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =DPProvider.Name,
                                                  Order = 2

                                              }
                                               ,
                                          new BreadCrumbModel()
                                              {
                                                  Text =textItem,
                                                  Order = 3
                                                
                                                 
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

      
            return View("LandingDataProvider", item);


        }
        protected Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.ListViewModel LoadData(Model.Core.Party.Filter filter, string type = "")
        {
            var result = GetQueryResult(filter, type);
            ViewData["total"] = result.TotalCount;
            #region Actions

            var actions = new List<Noqoush.AdFalcon.Web.Controllers.Model.Action>
                              {
                                  new Model.Action()
                                      {
                                          ActionName = "Delete",
                                          ClassName = "delete-button",
                                          Type = ActionType.Submit,
                                          DisplayText = ResourcesUtilities.GetResource("Archive", "PMPDeal"),
                                          ExtraPrams=ResourcesUtilities.GetResource("SelectConfirmation", "Party"),
                                          ExtraPrams2 = ResourcesUtilities.GetResource("Archive", "Confirmation"), // like are u sure ?
                                      },
                                  new  Model.Action()
                                      {
                                          ActionName = !string.IsNullOrEmpty(type) && type.ToLower() =="employee" ?"Employee":"BusinessPartner",
                                          ClassName = "primary-btn",
                                          Type = ActionType.Link,
                                           ControllerName="Party",
                                      DisplayText = ResourcesUtilities.GetResource("AddNewParty", "Commands")

                                  }
                              };

            #endregion

            return new Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.ListViewModel()
            {
                Items = result.Items,
                //TopActions = actions,
                //BelowAction = actions,

            };
        }
        protected PartyListResultDto GetQueryResult(Model.Core.Party.Filter filter, string type = "")
        {
            var criteria = GetCriteria(filter, type);
            var result = partyService.QueryByCriteriaForDPPartner(criteria);
            return result;
        }
        protected DPPartnerCriteria GetCriteria(Model.Core.Party.Filter filter, string type = "")
        {
            if (filter == null)
                filter = GetDefualtFilter();
            var criteria = new DPPartnerCriteria
            {
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
               Name=  filter.Name,
                Visible = true,
            
               
                //StatusId = filter.StatusId
            };

            
       


            //criteria.Type = PartyType.BusinessPartner;
            return criteria;
        }
        protected Model.Core.Party.Filter GetDefualtFilter()
        {
            var filter = new Model.Core.Party.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int)1 : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int)Config.PageSize : Convert.ToInt32(Request.Form["size"]);
           
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["PartyName"]) ? "" : Request.Form["PartyName"];
         
            return filter;
        }


        #endregion
    }
}
