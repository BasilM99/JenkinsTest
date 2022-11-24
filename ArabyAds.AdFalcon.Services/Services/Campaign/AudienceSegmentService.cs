using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.Framework.DomainServices.Localization.Repositories;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Common.UserInfo;
using ArabyAds.Framework;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Domain.Model.Campaign.Targeting;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.Framework.ExceptionHandling.Exceptions;
using ArabyAds.Framework.Resources;
using System.Linq.Expressions;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.Framework.Utilities;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;

namespace ArabyAds.AdFalcon.Services.Services.Campaign
{

    public class AudienceSegmentService : IAudienceSegmentService
    {
        private readonly IAudienceSegmentRepository audianSegRep = null;
        private readonly IContextualSegmentRepository contextualSegRep = null;
        private readonly IBusinessPartnerRepository busiRep = null;
        private  string codeForFirstParty= "fpaud";
        private string codeForFirstPartyContextual = "fpcd";
        private readonly ICampaignRepository campRep = null;
        private readonly IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository = null;
        private IDPPartnerRepository _DPPartnerRepository = null;
        private IContextualPartnerRepository _ContextualPartnerRepository = null;
        private IAdvertiserAccountRepository _AdvertiserAccountRepository = null;
        public AudienceSegmentService(IDPPartnerRepository DPPartnerRepository, IAudienceSegmentRepository sugAudianceRe, IBusinessPartnerRepository rep, ICampaignRepository CampaingRep, IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository, IAdvertiserAccountRepository AdvertiserAccountRepository, IContextualPartnerRepository ContextualPartnerRepository, IContextualSegmentRepository ContextualSegmentRepository)
        {
            audianSegRep = sugAudianceRe;
            campRep = CampaingRep;
            busiRep = rep;

            _AdvertiserAccountRepository = AdvertiserAccountRepository;
            this._AccountPortalPermissionsRepository = _AccountPortalPermissionsRepository;
            _DPPartnerRepository = DPPartnerRepository;
          
            if (JsonConfigurationManager.AppSettings["codeForFirstParty"]!=null && !string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["codeForFirstParty"]))
            {

                codeForFirstParty = JsonConfigurationManager.AppSettings["codeForFirstParty"];

            }

            if (JsonConfigurationManager.AppSettings["codeForFirstPartyContextual"] != null && !string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["codeForFirstPartyContextual"]))
            {

                codeForFirstPartyContextual = JsonConfigurationManager.AppSettings["codeForFirstPartyContextual"];

            }

            _ContextualPartnerRepository = ContextualPartnerRepository;
            contextualSegRep = ContextualSegmentRepository;
        }

        public void SaveContextualSegmentTargeting(AdGroup adGroup, string groupContextualString, TargetingType Type)
        {
            var contextualSegmentTargeting = new ContextualSegmentTargeting();
            var campaign= adGroup.Campaign;

            if (!string.IsNullOrEmpty(groupContextualString))
            {
                var contextualObjectList = contextualSegmentTargeting.GetContextualObject(groupContextualString);
                if (contextualObjectList.Count > 0)
                {
                    foreach (var contextualObject in contextualObjectList)
                    {
                        contextualObject.Type = Type;
                        campaign.AddGroupTargeting(adGroup, contextualObject);
                    }
                }
            }
        }

        public List<TreeDto> GetByDataProvider(GetByDataProviderRequest request)
        {
            List<AudienceSegment> list = audianSegRep.Query(M => M.Provider.ID == request.Id).Where(M => M.IsDeleted == false).OrderBy(M => M.Provider.Name).ToList();
            var dataProvider = busiRep.Query(M => M.ID == request.Id).SingleOrDefault();

            var result = BuildTreeViewCustom(list, request.showNotSelectable);

            return result;
        }
        public List<TreeDto> GetByDataProviderWithPrice(GetByDataProviderRequest request)
        {
            List<AudienceSegment> list = audianSegRep.Query(M => M.Provider.ID == request.Id).Where(M => M.IsDeleted == false).OrderBy(M => M.Provider.Name).ToList();
            var dataProvider = busiRep.Query(M => M.ID == request.Id).SingleOrDefault();

            var result = BuildTreeView(list, request.showNotSelectable, true, true);

            return result;
        }

        public List<TreeDto> GetAll(ValueMessageWrapper<int?> CampainId)
        {
            List<AudienceSegment> list = null;
            var FirstDpPartner = _DPPartnerRepository.Query(M => M.Code == codeForFirstParty).SingleOrDefault();
            if (CampainId.Value.HasValue)
            {
                var camp = campRep.Get(CampainId.Value.Value);
                list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true &&(M.Advertiser==null || M.Advertiser.ID==camp.AdvertiserAccount.ID) && (M.Provider.HasAdvertiserBlock == false || (  M.Provider.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID))) && ((M.Provider.HasAccountWhite == true   && M.Provider.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID)) ||M.Provider.ID== FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
            }
            else
            {
                if (Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>() != null && Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.HasValue)
                {
                    list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true && ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value)) || M.Provider.ID == FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
                    if (list != null && list.Count > 0)
                        list = list.Where(M => M.Account == null || M.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).ToList();
                }
                else
                {

                    list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true && ((M.Provider.HasAccountWhite == true ) || M.Provider.ID == FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
                }
            }

            //var DPPartnerlist = list.Select(M => M.Provider).Where(M => !M.IsDeleted && M.IsExternalProvider != true).Distinct<DPPartner>();



            var childs = BuildTreeView(list, true);
            return childs;
        }

        public List<TreeDto> GetAllForContextual(ValueMessageWrapper<int?> CampainId)
        {
            List<ContextualSegment> list = null;
            var FirstDpPartner = _ContextualPartnerRepository.Query(M => M.Code == codeForFirstPartyContextual).SingleOrDefault();
            if (CampainId.Value.HasValue)
            {
                var camp = campRep.Get(CampainId.Value.Value);
                list = contextualSegRep.Query(M => !M.IsDeleted && (M.SubType != "safefrom" &&  M.SubType != "safety")   &&  !M.Provider.IsDeleted && (M.Advertiser == null || M.Advertiser.ID == camp.AdvertiserAccount.ID) && (M.Provider.HasAdvertiserBlock == false || (M.Provider.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID))) /*|| ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID)) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
            }
            else
            {
                if (Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>() != null && Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.HasValue)
                {
                    list = contextualSegRep.Query(M => !M.IsDeleted && (M.SubType != "safefrom" && M.SubType != "safety")  && !M.Provider.IsDeleted  /*|| ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value)) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
                    if (list != null && list.Count > 0)
                        list = list.Where(M => M.Account == null || M.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).ToList();
                }
                else
                {

                    list = contextualSegRep.Query(M => !M.IsDeleted && (M.SubType != "safefrom" && M.SubType != "safety") && !M.Provider.IsDeleted  /*|| ((M.Provider.HasAccountWhite == true) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
                }
            }

           // var DPPartnerlist = list.Select(M => M.Provider).Where(M => !M.IsDeleted && M.IsExternalProvider != true).Distinct<DPPartner>();



            var childs = BuildTreeView(list, true);
            return childs;
        }

        public List<TreeDto> GetAllForContextualBrandSafty(ValueMessageWrapper<int?> CampainId)
        {
            List<ContextualSegment> list = null;
            var FirstDpPartner = _ContextualPartnerRepository.Query(M => M.Code == codeForFirstPartyContextual).SingleOrDefault();
            if (CampainId.Value.HasValue)
            {
                var camp = campRep.Get(CampainId.Value.Value);
                list = contextualSegRep.Query(M => !M.IsDeleted &&(M.SubType == "root" ||  M.SubType== "safefrom" || M.SubType == "safety") && !M.Provider.IsDeleted  && (M.Advertiser == null || M.Advertiser.ID == camp.AdvertiserAccount.ID) && (M.Provider.HasAdvertiserBlock == false || (M.Provider.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID))) /*|| ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID)) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
            }
            else
            {
                if (Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>() != null && Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.HasValue)
                {
                    list = contextualSegRep.Query(M => !M.IsDeleted &&  (M.SubType == "root"  ||  M.SubType == "safefrom" || M.SubType == "safety") && !M.Provider.IsDeleted /*|| ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value)) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
                    if (list != null && list.Count > 0)
                        list = list.Where(M => M.Account == null || M.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId.Value).ToList();
                }
                else
                {

                    list = contextualSegRep.Query(M => !M.IsDeleted && (M.SubType == "root" ||  M.SubType == "safefrom" || M.SubType == "safety") && !M.Provider.IsDeleted /*|| ((M.Provider.HasAccountWhite == true) || M.Provider.ID == FirstDpPartner.ID)*/).OrderBy(M => M.Provider.Name).ToList();
                }
            }

            // var DPPartnerlist = list.Select(M => M.Provider).Where(M => !M.IsDeleted && M.IsExternalProvider != true).Distinct<DPPartner>();



            var childs = BuildTreeView(list, true);
            return childs;
        }




    



        public AudienceSegmentDto get(ValueMessageWrapper<int> id)
        {
            var item = audianSegRep.Get(id.Value);
            var dto = MapperHelper.Map<AudienceSegmentDto>(item);
            if (item.Parent != null)
                dto.ParentName = item.Parent.Name.ToString();
            AudienceSegmentTargeting targ = new AudienceSegmentTargeting();
            dto.Path = targ.AudiencePath(id.Value);
            if (item.Parent != null&&  item.Parent.Provider!=null  && item.Parent.Provider.Code== targ.getFirstPartyCode())
            {
                dto.recency = 65535;
                dto.showrecency = true;
                    
                    }
            return dto;
        }


        public AudienceSegmentDto getContextual(ValueMessageWrapper<int> id)
        {
            var item = contextualSegRep.Get(id.Value);
            var dto = MapperHelper.Map<AudienceSegmentDto>(item);
            if (item.Parent != null)
                dto.ParentName = item.Parent.Name.ToString();
            ContextualSegmentTargeting targ = new ContextualSegmentTargeting();
            dto.Path = targ.AudiencePath(id.Value);
            if (item.Parent != null && item.Parent.Provider != null && item.Parent.Provider.Code == targ.getFirstPartyCode())
            {
                dto.recency = 65535;
                dto.showrecency = true;

            }
            return dto;
        }

        public ValueMessageWrapper<bool> Save(AudienceSegmentDto obj)
        {

            AudienceSegment item = null;



            if (obj.ID == 0)
            {
                if (audianSegRep.Query(x => x.ID == obj.ParentId && !x.IsDeleted).Count() == 0 && audianSegRep.Query(x => x.Provider.ID == obj.ProviderId && !x.IsDeleted).Count() > 0)
                {
                    var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentSuperRootViloate"));
                    throw error;
                }

                item = audianSegRep.Query(x => x.Code == obj.CodeUQ).FirstOrDefault();
                if (item != null)
                {
                    var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentDuplicateCode"));
                    throw error;
                }
                item = MapperHelper.Map<AudienceSegment>(obj);
                if (item == null)
                {
                    throw new Exception("item connet be null");
                }
                foreach (var localizedValue in item.Name.Values)
                {
                    if (!string.IsNullOrEmpty(item.Name.Value))
                    {
                        item.Name.Value = item.Name.Value.Trim();
                    }
                    localizedValue.LocalizedString = item.Name;
                }
            }
            else
            {

                //if (audianSegRep.Query(x => x.ID == obj.ParentId && !x.IsDeleted).Count() == 0)
                //{
                //    //item = audianSegRep.Get(obj.ID);

                //    //item.IsPermissionNeed = obj.IsPermissionNeed;
                //    var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentSuperRootViloate"));
                //    throw error;
                //}

                item = audianSegRep.Query(x => x.Code == obj.CodeUQ && x.ID != obj.ID).FirstOrDefault();

                if (item != null)
                {
                    var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentDuplicateCode"));
                    throw error;
                }

                item = audianSegRep.Get(obj.ID);
                if (item == null)
                {
                    throw new Exception("Invalid Id");
                }


                // if(item.Parent.ToString() == item.Name.Values[0].Value)
                //{
                //    throw new Exception("Invalid Id");
                //}
                if (item.Parent != null)
                {
                    //if(obj.ParentName == null)
                    //{
                    //    var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentSuperRootViloate"));
                    //    throw error;
                    //}

                    item.Price = obj.Price;
                    item.Description = obj.Description;
                    item.Code = obj.CodeUQ;
                    item.IsDeleted = obj.IsDeleted;
                    item.OperatorSegmentCode = obj.OperatorSegmentCode;
                    item.Selectable = obj.IsSelectedable;


                }
                else
                {

                    item.Description = obj.Description;
                    item.Code = obj.CodeUQ;
                    // item.IsDeleted = obj.IsDeleted;
                    item.OperatorSegmentCode = obj.OperatorSegmentCode;

                }
                if (obj.ParentId > 0)
                {

                    if (item.Parent == null)
                    {
                        var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentSuperRootViloate"));
                        throw error;
                    }
                    if (obj.ParentId == item.ID)
                    {
                        //throw new Exception("Invalid Parent");
                        var error = new BusinessException(null, null, ResourceManager.Instance.GetResource("AudienceSegmentSuperRootChangeViloate"));
                        throw error;
                    }
                    item.Parent = new AudienceSegment { ID = obj.ParentId };
                }
                else
                {

                    item.Parent = null;
                }

                item.IsPermissionNeed = obj.IsPermissionNeed;
                item.Name.Values[0].Value = obj.Name.Values[0].Value;
                item.Name.Values[1].Value = obj.Name.Values[1].Value;
            }
            item.Account = new Domain.Model.Account.Account { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value };
            item.User = new Domain.Model.Account.User { ID = OperationContext.Current.UserInfo<AdFalconUserInfo>().UserId.Value };

            if (item.Advertiser!=null && item.ID==0)
            {
                item.Advertiser = _AdvertiserAccountRepository.Get(item.Advertiser.ID);
                item.BinIndex = item.CalculateBinIndex(item.Advertiser.ID);
            }
            
            audianSegRep.Save(item);



            return ValueMessageWrapper.Create(true);
        }
        public string AudiencePath(int id)
        {
            var audianceSeq = audianSegRep.Get(id);
            string AudiencePathStr = string.Empty;
            if (audianceSeq.Parent != null)
            {
                AudiencePathStr = AudiencePath(audianceSeq.Parent.ID) + ">";



            }
            AudiencePathStr = AudiencePathStr + audianceSeq.Name.Value;
            return AudiencePathStr;
        }
     
        public string getAudianceSegmentsByDataProviderForExternal(GetByDataProviderExtRequest request)
        {
            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => c.Advertiser != null && c.Advertiser.ID == request.IdAccAdv && c.Provider.ID == request.Id && c.IsDeleted == false;

            List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

            var returnList = list.Select(campaign => MapperHelper.Map<AudienceSegmentDto>(campaign)).ToList();
            var integrationId = "";
            if (returnList != null && returnList.Count > 0)
            {
                var arr = returnList.Select(x => x.IntegrationId).ToArray(); ;

                integrationId = string.Join(",", arr);

            }
            return integrationId;
        }
        public IList<AudienceSegmentDto> getAudianceSegmentsByDataProvider(GetAudienceSegByDataProviderRequest request)
        {

            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => (c.Name.Values.Any(v => v.Value.StartsWith(request.q))) && c.Provider.ID == request.Id && c.IsDeleted == false;

            List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

            var returnList = list.Select(campaign => MapperHelper.Map<AudienceSegmentDto>(campaign)).ToList();
            returnList = returnList.OrderBy(M => M.ParentId).ToList();
            return returnList;
        }
        public IList<AudienceSegmentDto> getAudianceSegmentsByDataProviderToWrite(GetAudienceSegByDataProviderRequest request)
        {

            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => (c.Name.Values.Any(v => v.Value.StartsWith(request.q))) && c.Provider.ID == request.Id && c.IsDeleted == false;

            List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

            var returnList = list.Select(campaign => MapperHelper.Map<AudienceSegmentDto>(campaign)).ToList();
            returnList = returnList.OrderBy(M => M.ParentId).ToList();
         
           var order= CountLevel(returnList);
           // returnList = returnList.OrderBy(M => M.ParentId).ToList().OrderBy(M => M.Level).ToList();

            return order;
        }

        private List<AudienceSegmentDto> CountLevel(IEnumerable<AudienceSegmentDto> Audiences)
        {
            List<TreeDto> baseTreeView = new List<TreeDto>();
            var itemsParent = Audiences.Where(p => p.ParentId == 0).OrderBy(M=>M.Name.GetValue()).ToList();
            List<AudienceSegmentDto> OrderResults = new List<AudienceSegmentDto>();


            foreach (var item in itemsParent)
            {
               



          
                item.Level = 1;
                item.Names = new List<string>();
                item.Names.Add(item.Name.GetValue());
                OrderResults.Add(item);
                var orderChild= CountLevelChild(item, Audiences);
                OrderResults.AddRange(orderChild);

            }
            return OrderResults;
            
        }
        private List<AudienceSegmentDto>  CountLevelChild (AudienceSegmentDto parent, IEnumerable<AudienceSegmentDto> Audiences)
        {
            List<TreeDto> tree = new List<TreeDto>();
            IEnumerable<AudienceSegmentDto> filteredItems = Audiences.Where(p => p.ParentId>0 && p.ParentId == parent.ID);
            List<AudienceSegmentDto> results = new List<AudienceSegmentDto>();
            if (filteredItems != null && filteredItems.Count() != 0)
            {
                var values = filteredItems.ToList().OrderBy(M => M.Name.GetValue()).ToList();

                foreach (var item in values)
                {
                    if (item.IsPermissionNeed && !Domain.Configuration.IsAdmin && !_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.AudianceSegmentUsagePermission))
                    {
                        continue;
                    }

                  
                    item.Level = parent.Level + 1;
                    item.Names = new List<string>();
                    foreach(var str in parent.Names)
                    item.Names.Add(str);
                    item.Names.Add(item.Name.GetValue());
                    results.Add(item);
                   var itemsc= CountLevelChild(item, Audiences);
                    var result =itemsc.OrderBy(M => M.Name.GetValue()).ToList();
                    results.AddRange(result);


                }



            }
            return results;



        }

        private List<TreeDto> BuildTreeView(IEnumerable<Segment> Audiences, bool showNotSelectable = false, bool showPrice = false, bool showLocked = true)
        {
            List<TreeDto> baseTreeView = new List<TreeDto>();
            var orderedList = Audiences.Where(p => p.Parent == null).OrderBy(M => M.Name.GetValue()).OrderBy(m => m.Provider.Order).ToList();
            foreach (var item in orderedList)
            {

                if (!item.Selectable)
                {
                    if (!showNotSelectable) continue;
                }

                if (item.IsPermissionNeed && !Domain.Configuration.IsAdmin && !_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.AudianceSegmentUsagePermission))
                {
                    continue;
                }

                TreeDto Audience = new TreeDto {   CustomValue = item.Provider.ID, state = !item.Selectable ? "locked" : "", Id = item.ID.ToString(), Name = MapperHelper.Map<LocalizedStringDto>(item.Name), style = !item.Selectable ? "color:#969696;" : "" };

                BuildChildrenTreeView(Audience, Audiences, showLocked, showPrice);
                if (Audience.Childs != null && Audience.Childs.Count > 0)
                    baseTreeView.Add(Audience);

            }

            return baseTreeView;
        }

        private List<TreeDto> BuildTreeViewCustom(IEnumerable<Segment> Audiences, bool showNotSelectable = false)
        {
            List<TreeDto> baseTreeView = new List<TreeDto>();

            foreach (var item in Audiences.Where(p => p.Parent == null))
            {
                if (!item.Selectable)
                {
                    if (!showNotSelectable) continue;
                }

               

                TreeDto Audience = new TreeDto { CustomValue = item.Provider.ID, state = !item.Selectable ? "" : "", Id = item.ID.ToString(), Name = MapperHelper.Map<LocalizedStringDto>(item.Name), style = !item.Selectable ? "color:#969696;" : "" };

                BuildChildrenTreeView(Audience, Audiences, false);
                baseTreeView.Add(Audience);

            }

            return baseTreeView;
        }
        private void BuildChildrenTreeView(TreeDto parent, IEnumerable<Segment> Audiences, bool showlocked = true, bool showPrice = false)
        {
            List<TreeDto> tree = new List<TreeDto>();
            IEnumerable<Segment> filteredItems = Audiences.Where(p => p.Parent != null && p.Parent.ID.ToString() == parent.Id);

            if (filteredItems != null && filteredItems.Count() != 0)
            {
                parent.Childs = new List<TreeDto>();
            }
            string locedstring = "locked";

            string colorGray = "color:#969696;";
            if (!showlocked)
            {
                locedstring = "";

                colorGray = "color:#969696;";
            }
            foreach (var item in filteredItems)
            {
                if (item.IsPermissionNeed && !Domain.Configuration.IsAdmin && !_AccountPortalPermissionsRepository.checkAdPermissions(PortalPermissionsCode.AudianceSegmentUsagePermission))
                {
                    continue;
                }

                TreeDto child = new TreeDto
                {
                    Id = item.ID.ToString(),
                    CustomValue = item.Provider.ID,
                    
                    state = !item.Selectable ? locedstring : "",
                    style = !item.Selectable ? colorGray : "",
                    Name = MapperHelper.Map<LocalizedStringDto>(item.Name)
                };

                if (showPrice)
                {
                    //child.Name.Values[0] = new LocalizedValueDto { Culture = "en-US", Value = child.Name.Get("en-US") + "-" + (item.Price * 1000).ToString("F2") + "$" };
                    //child.Name.Values[1] = new LocalizedValueDto { Culture = "ar-JO", Value = child.Name.Get("ar-JO") + "-" + (item.Price * 1000).ToString("F2") + "$" };

                    child.Name.SetValue(child.Name.Get("en-US") + "-" + (item.Price * 1000).ToString("F2") + "$", "en-US");
                    child.Name.SetValue(child.Name.Get("en-US") + "-" + (item.Price * 1000).ToString("F2") + "$", "ar-JO");

                }
                
                BuildChildrenTreeView(child, Audiences, showlocked, showPrice);
                if (!(child.Childs == null || child.Childs.Count == 0))
                {
                    child.state = locedstring;
                    child.style = colorGray;
                }
                parent.Childs.Add(child);
            }
            if(parent.Childs!=null && parent.Childs.Count>0)
            parent.Childs= parent.Childs.OrderBy(M => M.Name.GetValue()).ToList();
        }
   
    
    
    
    }
}
