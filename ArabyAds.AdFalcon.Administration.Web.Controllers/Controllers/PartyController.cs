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
using Noqoush.AdFalcon.Web.Controllers.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Creative;

namespace Noqoush.AdFalcon.Administration.Web.Controllers.Controllers
{
    //[AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
    [PermissionsAuthorize(Permission = PortalPermissionsCode.PMPDeal, Roles = "Administrator,adops,AccountManager")]
    //[DenyRole(Roles = "AppOps")]
    public class PartyController : Noqoush.AdFalcon.Web.Controllers.Controllers.PartyController
    {
        private ITileImageService _tileImageService;
        private IDocumentService _DocumentService;
        protected ICostModelWrapperService _CostModelWrapperService;

        public PartyController(IPartyService partyService, ILookupService lookupService, ITileImageService tileImageService, IDocumentService _DocumentService, ICostModelWrapperService costModelWrapperService) : base(partyService, lookupService)
        {
            _CostModelWrapperService = costModelWrapperService;

            _tileImageService = tileImageService;
            this._DocumentService = _DocumentService;
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
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        public ActionResult BusinessPartners()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                          new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("BusinessPartners", "Menu"),
                                                  Order = 1,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            var model = LoadData(null, "BusinessPartners");

            model.BusinessPartnerTypes = GetList(LookupNames.BusinessPartnerType, null);
            return View("BusinessPartners", model);
        }
        [GridAction(EnableCustomBinding = true)]
        [RequireHttps(Order = 1)]
        public ActionResult _BusinessPartners()
        {
            var result = GetQueryResult(null, "BusinessPartners");
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [RequireHttps(Order = 1)]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult BusinessPartners(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                partyService.DeleteParties(checkedRecords);
            }

            return RedirectToAction("BusinessPartners");
        }

        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        [RequireHttps(Order = 1)]
        public ActionResult Employees()
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                         new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Employees","Menu"),
                                                  Order = 1
                                              }

                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            var model = LoadData(null, "employee");
            return View(model);
        }
        [GridAction(EnableCustomBinding = true)]
        [RequireHttps(Order = 1)]
        public ActionResult _Employees()
        {
            var result = GetQueryResult(null, "employee");
            ViewData["total"] = result.TotalCount;
            return View(new GridModel
            {
                Data = result.Items,
                Total = Convert.ToInt32(result.TotalCount)
            });
        }
        [RequireHttps(Order = 1)]
        [AcceptVerbs(HttpVerbs.Post)]
        [DenyRole(Roles = "AccountManager")]

        public virtual ActionResult Employees(int[] checkedRecords)
        {

            if (!string.IsNullOrWhiteSpace(Request.Form["Delete"]))
            {
                partyService.DeleteParties(checkedRecords);
            }

            return RedirectToAction("Employees");
        }

        // [RequireHttps(Order = 1)]

        public ActionResult IndexNoHttps(Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.Filter filter)
        {
            return View(LoadData(filter));
        }



        public ActionResult Search(Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.Filter filter)
        {
            return PartialView(LoadData(filter));
        }

        private Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.PartyViewModel GetPartyModel(int? id, string type)
        {
            var model = new Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.PartyViewModel();
            int item_id = id.HasValue ? id.Value : 0;


            if (type.ToLower() == "employee")
            {

                model.PartyDto = item_id < 1 ? null : partyService.GetEmployee(item_id);
                model.JobPositions = GetList(LookupNames.JobPosition, model.PartyDto == null ? (int?)0 : (model.PartyDto as EmployeeDto).JobPositionId);
                model.ViewName = "Employee";
                model.SaveAction = "CreateEmployee";
            }
            else
            {

                model.PartyDto = item_id < 1 ? null : partyService.GetBusinessPartner(item_id);
                model.ViewName = "BusinessPartner";
                model.SaveAction = "CreateBusinessPartner";
                string value = "0";
                if (model.PartyDto != null)
                {
                    if ((model.PartyDto as BusinessPartnerDto).AuctionPricePricingUnitId != null)
                        value = (model.PartyDto as BusinessPartnerDto).AuctionPricePricingUnitId.ToString();
                    else
                        value = string.Empty;
                }
                if (value == "0")
                {
                    value = "1";
                }
                List<SelectListItem> PriceModels = new List<SelectListItem>();
                PriceModels.Add(new SelectListItem() { Value = "1", Text = "CPM", Selected = value == "1" });
                PriceModels.Add(new SelectListItem() { Value = "2", Text = "CPI", Selected = value == "2" });
                PriceModels.Add(new SelectListItem() { Value = "3", Text = "CPI micros", Selected = value == "3" });

                if (model.PartyDto != null)
                {
                    if ((model.PartyDto as BusinessPartnerDto).AuctionPriceEncryptionAlgorithmId != null)
                        value = (model.PartyDto as BusinessPartnerDto).AuctionPriceEncryptionAlgorithmId.ToString();
                    else
                        value = string.Empty;
                }
                if (value == "0")
                {
                    value = "0";
                }
                List<SelectListItem> PriceEncryptionAlgorithm = new List<SelectListItem>();
                PriceEncryptionAlgorithm.Add(new SelectListItem() { Value = "", Text = "No Algorithem", Selected = value == "0" });
                PriceEncryptionAlgorithm.Add(new SelectListItem() { Value = "1", Text = "HMAC_SHA1", Selected = value == "1" });
                PriceEncryptionAlgorithm.Add(new SelectListItem() { Value = "2", Text = "Blowfish", Selected = value == "2" });

                model.PriceModels = PriceModels;
                model.PriceEncryptionAlgorithm = PriceEncryptionAlgorithm;
                model.BusinessPartnerTypes = GetList(LookupNames.BusinessPartnerType, model.PartyDto == null ? (int?)0 : (model.PartyDto as BusinessPartnerDto).BusinessPartnerTypeId);
                TileImageSizeDto sizeDTO = _tileImageService.GetSizeByParentId(1);

                int? documentId = model.PartyDto != null ? (model.PartyDto as BusinessPartnerDto).documentId : null;

                DocumentDto document = null;
                if (documentId.HasValue)
                    document = _DocumentService.Get((int)documentId);

                var creativeUnit = new CreativeUnitViewModel()
                {
                    DocumentId = documentId,
                    Content = document != null ? document.Content.ToString() : string.Empty,
                    DisplayText = ResourcesUtilities.GetResource("Icon", "Global"),
                    CreativeUnitDto = null,
                    DeviceType = DeviceTypeEnum.Any,
                    AdTypeId = 1,
                    TileImageDocumentDto = new TileImageDocumentDto
                    {
                        TileImageSize = new TileImageSizeDto
                        {
                            Formats = sizeDTO.Formats
                        }
                    },
                    Name = "testing"
                };



                model.creativeUnit = creativeUnit;
                model.DemandType = partyService.GetDemandBusinesPartner();
                model.SupplyType = partyService.GetSupplyBusinesPartner();

                model.DataProviderType = partyService.GetDPBusinesPartner();
            }

            return model;


        }


        #endregion
        #region Create
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        [RequireHttps(Order = 1)]
        public ActionResult Employee(int? id)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                         new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Employees","Menu"),
                                                  Order = 1,
                                                  Url = Url.Action("Employees")
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AddEmployees","Menu"),
                                                  Order = 2,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            return View(GetPartyModel(id, "Employee"));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult Employee(Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.EmployeePartySaveModel party)
        {
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                         new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("Employees","Menu"),
                                                  Order = 1,
                                                  Url = Url.Action("Employees")
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text =ResourcesUtilities.GetResource("AddEmployees","Menu"),
                                                  Order = 2,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion

            int? id = party.PartyDto.ID;

            string Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), "Employee");
            MessagesType type = MessagesType.Success;
            try
            {

                id = partyService.SaveEmployee(party.PartyDto);

            }
            catch (BusinessException exception)
            {
                Message = exception.Errors.FirstOrDefault().Message;
                type = MessagesType.Error;
            }
            AddMessages(Message, type);
            return View(GetPartyModel(id, "Employee"));
            //return RedirectToAction("Employee", new { id = id });


        }


        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        public ActionResult BusinessPartner(int? id)
        {

            var model = GetPartyModel(id, "BusinessPartner");
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                         new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("BusinessPartners","Menu"),
                                                  Order = 1,
                                                  Url = Url.Action("BusinessPartners")
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text = !id.HasValue ? ResourcesUtilities.GetResource("AddBusinessPartners","Menu"):string.Format("{0} {1} {2}",ResourcesUtilities.GetResource("Edit","Commands") ,model.PartyDto.Name,ResourcesUtilities.GetResource("BusinessPartner","Party")),
                                                  Order = 2,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(model);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequireHttps(Order = 1)]
        [AuthorizeRole(Roles = "Administrator,adops,AccountManager")]
        [DenyRole(Roles = "AccountManager")]

        public ActionResult BusinessPartner(Noqoush.AdFalcon.Web.Controllers.Model.Core.Party.BusinessPartnerPartySaveModel party)
        {

            int? id = party.PartyDto.ID;
            bool ss = ModelState.IsValid;
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }


            string Message = string.Format(ResourcesUtilities.GetResource("savedSuccessfully", "Global"), "Business Partner");
            MessagesType type = MessagesType.Success;
            try
            {

                id = partyService.SaveBusinessPartner(party.PartyDto);
                var userOb = Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>();
                userOb.SwitchAccountSet = true;
                Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>(userOb);
            }
            catch (BusinessException exception)
            {
                Message = exception.Errors.FirstOrDefault().Message;
                type = MessagesType.Error;
            }
            AddMessages(Message, type);
            #region BreadCrumb

            var breadCrumbLinks = new List<BreadCrumbModel>
                                      {
                                         new BreadCrumbModel()
                                              {
                                                  Text = ResourcesUtilities.GetResource("BusinessPartners","Menu"),
                                                  Order = 1,
                                                  Url = Url.Action("BusinessPartners")
                                              },
                                           new BreadCrumbModel()
                                              {
                                                  Text = type ==MessagesType.Error && !id.HasValue? ResourcesUtilities.GetResource("AddBusinessPartners","Menu"):string.Format("{0} {1} {2}", ResourcesUtilities.GetResource("Edit","Commands"), party.PartyDto.Name, ResourcesUtilities.GetResource("BusinessPartner", "Party")),
                                                  Order = 2,
                                              }
                                      };

            ViewData["BreadCrumbLinks"] = breadCrumbLinks;

            #endregion
            return View(GetPartyModel(id, "BusinessPartner"));

        }


        [RequireHttps(Order = 1)]
        public ActionResult Create(string party, int? id, string Message = null, MessagesType type = MessagesType.Success)
        {

            if (!string.IsNullOrEmpty(Message)) AddMessages(Message, type);
            if (string.IsNullOrEmpty(party)) party = "businesspartner";

            return View(GetPartyModel(id, party));
        }


        #endregion

        #region ipRange

        [GridAction]
        [RequireHttps(Order = 1)]
        public ActionResult Dummy()
        {
            return Content("");
        }
        [GridAction(EnableCustomBinding = true)]
        [RequireHttps(Order = 1)]
        public ActionResult DummySelect()
        {
            var result = new List<WhitleListIPDto>();
            return View(new GridModel
            {
                Data = result,
                Total = 0
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        [RequireHttps(Order = 1)]

        public ActionResult _DummyDelete(int id)
        {
            //Rebind the grid
            var result = new List<WhitleListIPDto>();
            return View(new GridModel(result));
        }

        #endregion
    }
}
