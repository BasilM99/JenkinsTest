using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework.DomainServices.Localization.Repositories;
using Noqoush.Framework.Persistence;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.Framework;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Core;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using Noqoush.Framework.Resources;
using System.Linq.Expressions;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Services.Campaign
{

    public class AudienceSegmentService : IAudienceSegmentService
    {
        private readonly IAudienceSegmentRepository audianSegRep = null;

        private readonly IBusinessPartnerRepository busiRep = null;
        private  string codeForFirstParty= "fpaud";
        private readonly ICampaignRepository campRep = null;
        private readonly IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository = null;
        private IDPPartnerRepository _DPPartnerRepository = null;

        private IAdvertiserAccountRepository _AdvertiserAccountRepository = null;
        public AudienceSegmentService(IDPPartnerRepository DPPartnerRepository, IAudienceSegmentRepository sugAudianceRe, IBusinessPartnerRepository rep, ICampaignRepository CampaingRep, IAccountPortalPermissionsRepository _AccountPortalPermissionsRepository, IAdvertiserAccountRepository AdvertiserAccountRepository)
        {
            audianSegRep = sugAudianceRe;
            campRep = CampaingRep;
            busiRep = rep;

            _AdvertiserAccountRepository = AdvertiserAccountRepository;
            this._AccountPortalPermissionsRepository = _AccountPortalPermissionsRepository;
            _DPPartnerRepository = DPPartnerRepository;
          
            if (System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"]!=null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"]))
            {

                codeForFirstParty = System.Configuration.ConfigurationManager.AppSettings["codeForFirstParty"];

            }
        }

        public List<TreeDto> GetByDataProvider(int Id, bool showNotSelectable = false)
        {
            List<AudienceSegment> list = audianSegRep.Query(M => M.Provider.ID == Id).Where(M => M.IsDeleted == false).OrderBy(M => M.Provider.Name).ToList();
            var dataProvider = busiRep.Query(M => M.ID == Id).SingleOrDefault();

            var result = BuildTreeViewCustom(list, showNotSelectable);

            return result;
        }
        public List<TreeDto> GetByDataProviderWithPrice(int Id, bool showNotSelectable = false)
        {
            List<AudienceSegment> list = audianSegRep.Query(M => M.Provider.ID == Id).Where(M => M.IsDeleted == false).OrderBy(M => M.Provider.Name).ToList();
            var dataProvider = busiRep.Query(M => M.ID == Id).SingleOrDefault();

            var result = BuildTreeView(list, showNotSelectable, true, true);

            return result;
        }

        public List<TreeDto> GetAll(int? CampainId)
        {
            List<AudienceSegment> list = null;
            var FirstDpPartner = _DPPartnerRepository.Query(M => M.Code == codeForFirstParty).SingleOrDefault();
            if (CampainId.HasValue)
            {
                var camp = campRep.Get(CampainId.Value);
                list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true &&(M.Advertiser==null || M.Advertiser.ID==camp.AdvertiserAccount.ID) && (M.Provider.HasAdvertiserBlock == false || (  M.Provider.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID))) && ((M.Provider.HasAccountWhite == true   && M.Provider.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID)) ||M.Provider.ID== FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
            }
            else
            {
                if (Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>() != null && Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.HasValue)
                {
                    list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true && ((M.Provider.HasAccountWhite == true && M.Provider.AccountWhiteList.Any(C => C.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value)) || M.Provider.ID == FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
                    if (list != null && list.Count > 0)
                        list = list.Where(M => M.Account == null || M.Account.ID == Framework.OperationContext.Current.UserInfo<Noqoush.Framework.UserInfo.IUserInfo>().AccountId.Value).ToList();
                }
                else
                {

                    list = audianSegRep.Query(M => !M.IsDeleted && !M.Provider.IsDeleted && M.Provider.IsExternalProvider != true && ((M.Provider.HasAccountWhite == true ) || M.Provider.ID == FirstDpPartner.ID)).OrderBy(M => M.Provider.Name).ToList();
                }
            }

            var DPPartnerlist = list.Select(M => M.Provider).Where(M => !M.IsDeleted && M.IsExternalProvider != true).Distinct<DPPartner>();



            var childs = BuildTreeView(list, true);
            return childs;
        }
        public AudienceSegmentDto get(int id)
        {
            var item = audianSegRep.Get(id);
            var dto = MapperHelper.Map<AudienceSegmentDto>(item);
            if (item.Parent != null)
                dto.ParentName = item.Parent.Name.ToString();
            AudienceSegmentTargeting targ = new AudienceSegmentTargeting();
            dto.Path = targ.AudiencePath(id);
            return dto;
        }

        public bool Save(AudienceSegmentDto obj)
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



            return true;
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
        private List<TreeDto> BuildTreeView(IEnumerable<AudienceSegment> Audiences, bool showNotSelectable = false, bool showPrice = false, bool showLocked = true)
        {
            List<TreeDto> baseTreeView = new List<TreeDto>();
            var orderedList = Audiences.Where(p => p.Parent == null).OrderBy(M => M.Name.GetValue()).OrderBy(m=>m.Provider.Order).ToList();
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

                TreeDto Audience = new TreeDto {  CustomValue = item.Provider.ID, state = !item.Selectable ? "locked" : "", Id = item.ID.ToString(), Name = MapperHelper.Map<LocalizedStringDto>(item.Name), style = !item.Selectable ? "color:#969696;" : "" };

                BuildChildrenTreeView(Audience, Audiences, showLocked, showPrice);
                if (Audience.Childs != null && Audience.Childs.Count > 0)
                    baseTreeView.Add(Audience);

            }

            return baseTreeView;
        }

        public string getAudianceSegmentsByDataProviderForExternal(int Id, int IdAccAdv)
        {
            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => c.Advertiser != null && c.Advertiser.ID == IdAccAdv && c.Provider.ID == Id && c.IsDeleted == false;

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
        public IList<AudienceSegmentDto> getAudianceSegmentsByDataProvider(int Id, string q, string cultures)
        {

            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => (c.Name.Values.Any(v => v.Value.StartsWith(q))) && c.Provider.ID == Id && c.IsDeleted == false;

            List<AudienceSegment> list = audianSegRep.Query(filter).ToList();

            var returnList = list.Select(campaign => MapperHelper.Map<AudienceSegmentDto>(campaign)).ToList();
            returnList = returnList.OrderBy(M => M.ParentId).ToList();
            return returnList;
        }
        public IList<AudienceSegmentDto> getAudianceSegmentsByDataProviderToWrite(int Id, string q, string cultures)
        {

            Expression<Func<AudienceSegment, bool>> filter = c => true;


            filter = c => (c.Name.Values.Any(v => v.Value.StartsWith(q))) && c.Provider.ID == Id && c.IsDeleted == false;

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


        private List<TreeDto> BuildTreeViewCustom(IEnumerable<AudienceSegment> Audiences, bool showNotSelectable = false)
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
        private void BuildChildrenTreeView(TreeDto parent, IEnumerable<AudienceSegment> Audiences, bool showlocked = true, bool showPrice = false)
        {
            List<TreeDto> tree = new List<TreeDto>();
            IEnumerable<AudienceSegment> filteredItems = Audiences.Where(p => p.Parent != null && p.Parent.ID.ToString() == parent.Id);

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

                parent.Childs.Add(child);
            }
            if(parent.Childs!=null && parent.Childs.Count>0)
            parent.Childs= parent.Childs.OrderBy(M => M.Name.GetValue()).ToList();
        }
    }
}
