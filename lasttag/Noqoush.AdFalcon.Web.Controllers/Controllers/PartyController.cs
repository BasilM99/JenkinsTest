using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using Noqoush.AdFalcon.Domain.Common.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Domain.Common.Repositories.Core;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Web.Controllers.Core;
using Noqoush.AdFalcon.Web.Controllers.Model;
using Noqoush.AdFalcon.Web.Controllers.Utilities;
using Noqoush.Framework;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Telerik.Web.Mvc;
using Noqoush.AdFalcon.Web.Controllers.Core.Security;
using Noqoush.AdFalcon.Web.Controllers.Model.User;
using Noqoush.AdFalcon.Domain.Common.Model.Account;
using Newtonsoft.Json;

namespace Noqoush.AdFalcon.Web.Controllers.Controllers
{
    [PermissionsAuthorize(Permission = PortalPermissionsCode.PMPDeal, Roles = "Administrator,adops,AccountManager")]
    //[DenyRole(AccountRoles = new AccountRole[] { AccountRole.DataProvider })]

    public class PartyController : AuthorizedControllerBase
    {
        protected IPartyService partyService;
        protected ILookupService lookupService;
        public PartyController(IPartyService partyService, ILookupService lookupService)
        {
            this.partyService = partyService;
            this.lookupService = lookupService;
        }
        public virtual ActionResult SSPSearch(Model.Core.Party.Filter filter)
        {
            return PartialView(LoadData(filter));
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
                TopActions = actions,
                BelowAction = actions,

            };
        }
        protected PartyListResultDto GetQueryResult(Model.Core.Party.Filter filter, string type = "")
        {
            var criteria = GetCriteria(filter, type);
            var result = partyService.QueryByCriteria(criteria);
            return result;
        }
        protected PartyCriteria GetCriteria(Model.Core.Party.Filter filter, string type = "")
        {
            if (filter == null)
                filter = GetDefualtFilter();
            var criteria = new PartyCriteria
            {
                Size = filter.size.HasValue ? filter.size.Value : Config.PageSize,
                Page = filter.page.HasValue ? filter.page.Value : 1,
                Name = filter.Prefix,
                ShowArchive = filter.ShowArchive,
                Visible = true,
                Type = string.IsNullOrEmpty(type) ? getPartyType(filter.id) : getPartyType(type),
                notInclud = string.IsNullOrWhiteSpace(Request.Form["SSPCheckedIDs"]) ? null : Request.Form["SSPCheckedIDs"].Split(',').Select(x => Convert.ToInt32(x)).ToList()
                //StatusId = filter.StatusId
            };

            if (!string.IsNullOrWhiteSpace(filter.IdPrefix))
            {
                criteria.Type = (PartyType)Enum.ToObject(typeof(PartyType), Convert.ToInt32(filter.IdPrefix));
            }
            if (!string.IsNullOrEmpty(filter.id))
            {
                switch (filter.id)
                {
                    case "DemandBusinesspartner":
                        criteria.Code = "DemandType";

                        break;
                    case "DataProviderBusinesspartner":
                        criteria.Code = "DataProviderType";

                        break;
                    case "SupplyBusinesspartner":
                        criteria.Code = "SupplyType";


                        break;

                    default:
                        criteria.Code = "";
                        break;
                }
            }


            //criteria.Type = PartyType.BusinessPartner;
            return criteria;
        }
        protected Model.Core.Party.Filter GetDefualtFilter()
        {
            var filter = new Model.Core.Party.Filter();
            filter.page = string.IsNullOrWhiteSpace(Request.Form["page"]) ? (int)1 : Convert.ToInt32(Request.Form["page"]);
            filter.size = string.IsNullOrWhiteSpace(Request.Form["size"]) ? (int)Config.PageSize : Convert.ToInt32(Request.Form["size"]);
            filter.id = string.IsNullOrWhiteSpace(Request.Form["Type"]) ? "" : Request.Form["Type"];
            filter.IdPrefix= string.IsNullOrWhiteSpace(Request.Form["IdPrefix"]) ? null :  Request.Form["IdPrefix"];
            filter.Name = string.IsNullOrWhiteSpace(Request.Form["PartyName"]) ? "" : Request.Form["PartyName"];
            filter.Prefix = string.IsNullOrWhiteSpace(Request.Form["Prefix"]) ? "" : Request.Form["Prefix"];
            filter.ShowArchive = string.IsNullOrWhiteSpace(Request.Form["ShowArchive"]) ? false : Convert.ToBoolean(Request.Form["ShowArchive"]);
            return filter;
        }
        protected PartyType? getPartyType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return (PartyType?)null;
            else
            {
                switch (type.ToLower())
                {
                    case "account":
                        {
                            return PartyType.Account;
                        }
                    case "employee":
                        {
                            return PartyType.Employee;
                        }
                    case "businesspartner":
                    case "demandbusinesspartner":
                    case "dataproviderbusinesspartner":
                    case "supplybusinesspartner":
                        {
                            return PartyType.BusinessPartner;
                        }
                    case "ssppartner":
                        {
                            return PartyType.SSP;
                        }
                    case "dsppartner":
                        {
                            return PartyType.DSP;
                        }
                    default:
                        {
                            return (PartyType?)null;
                        }
                }
            }
        }
   
        [GridAction(EnableCustomBinding = true)]
        //[RequireHttps(Order = 1)]
        public ActionResult _IndexNoHttps(string id, string Prefix, int? page, int? size)//,Model.Core.Party.Filter filter)
        {
            return GetPageItems(id, Prefix, page, size);
        }

        protected ActionResult GetPageItems(string id, string Prefix, int? page, int? size)
        {
            var filter = new Model.Core.Party.Filter { id = id, Prefix = Prefix, page = page, size = size };
            var result = GetQueryResult(filter);
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }

        [OutputCache(Duration = 3600, VaryByParam = "size")]
        public ActionResult _IndexNoHttpsSelect2Object(string id, string Prefix, int? page, int? size)//,Model.Core.Party.Filter filter)
        {
            return GetPageItemsSelect2Object(id, Prefix, page, size);
        }

        protected ActionResult GetPageItemsSelect2Object(string id, string Prefix, int? page, int? size)
        {
            var filter = new Model.Core.Party.Filter { id = id, Prefix = Prefix, page = page, size = size };
            var result = GetQueryResult(filter);
            int itemsLength = Convert.ToInt32(result.Items.Count());
            var JsonObject = new Object[itemsLength];
            

            int index = 0;
            foreach (var item in result.Items)
            {
                JsonObject[index] = new
                {
                    attributes = new
                    {
                        id = item.ID
                    },
                    data = item.Name,
                    children = new Object[0]
                };
                index++;
            }

            //var json = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);

            return Json(JsonObject);
        }
    }
}
