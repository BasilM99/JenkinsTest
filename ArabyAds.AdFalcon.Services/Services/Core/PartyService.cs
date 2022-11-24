using System;
using System.Collections.Generic;
using System.Linq;
using ArabyAds.AdFalcon.Domain.Model.Core;
using ArabyAds.AdFalcon.Domain.Repositories;
using ArabyAds.AdFalcon.Domain.Repositories.Campaign;
using ArabyAds.AdFalcon.Domain.Repositories.Core;
using ArabyAds.AdFalcon.Exceptions.Core;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Mapping;
using ArabyAds.AdFalcon.Domain.Model.Account;
using ArabyAds.Framework.Utilities;
using System.Net;
using ArabyAds.AdFalcon.Domain.Model.Campaign;
using NHibernate;
using NHibernate.Criterion;
using ArabyAds.Framework.Persistence;
using ArabyAds.AdFalcon.Domain.Model.Account.SSP;
using ArabyAds.AdFalcon.Domain.Repositories.Account.DPP;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account.DPP;
//using ArabyAds.AdFalcon.Base;
using ArabyAds.AdFalcon.Domain.Common.Model.Account;
using ArabyAds.AdFalcon.Domain.Common.Model.Campaign;
using ArabyAds.Framework.DomainServices.Localization;
using ArabyAds.Framework;

namespace ArabyAds.AdFalcon.Services.Services.Core
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IJobPositionRepository _jobPositionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IBusinessPartnerRepository _BusinessPartnerRepository;
        private readonly IAccountPartyDefineRepository _accountPartyDefineRepository;
        private readonly IDocumentRepository _documentRepository = null;

        private readonly IBusinessPartnerTypeRepository _BusinessPartnerTypeRepository;
        private readonly ISSPPartnerRepository _SSPPartnerRepository;
        private readonly IAppSiteRepository _AppSiteRepository;
        private readonly IDSPPartnerRepository _DSPPartnerRepository;
        private readonly IDPPartnerRepository _DPPartnerRepository;
        private readonly IImpressionLogRepository _ImpressionLogRepository;
        private readonly ICampaignRepository _CampaignRepository;
        private readonly IAudienceSegmentRepository audianSegRep = null;
        public PartyService(IImpressionLogRepository ImpressionLogRepository,  IAccountPartyDefineRepository accountPartyDefineRepository, 
            IPartyRepository partyRepository, IJobPositionRepository jobPositionRepository, IAccountRepository accountRepository,
            IBusinessPartnerRepository BusinessPartnerRepository, IEmployeeRepository EmployeeRepository,
            IDSPPartnerRepository DSPPartnerRepository, ISSPPartnerRepository SSPPartnerRepository,
            IBusinessPartnerTypeRepository BusinessPartnerTypeRepository, IAppSiteRepository AppSiteRepository,
            IDocumentRepository _documentRepository, IAudienceSegmentRepository seqRep, 
            IDPPartnerRepository DPartRepos, ICampaignRepository campRes)
        {
            this._partyRepository = partyRepository;
            this._jobPositionRepository = jobPositionRepository;
            this._accountRepository = accountRepository;
            this._accountPartyDefineRepository = accountPartyDefineRepository;

            this._EmployeeRepository = EmployeeRepository;
            this._BusinessPartnerRepository = BusinessPartnerRepository;

            this._documentRepository = _documentRepository;
            this._SSPPartnerRepository = SSPPartnerRepository;
            this._DSPPartnerRepository = DSPPartnerRepository;
            this._BusinessPartnerTypeRepository = BusinessPartnerTypeRepository;
            this._AppSiteRepository = AppSiteRepository;
            this._DPPartnerRepository = DPartRepos;
            this._ImpressionLogRepository = ImpressionLogRepository;
            this.audianSegRep = seqRep;
            this._CampaignRepository = campRes; 
        }
        public ValueMessageWrapper<int> GetDemandBusinesPartner()
        {


            return ValueMessageWrapper.Create(_BusinessPartnerTypeRepository.Query(M => M.Code == "DemandType").SingleOrDefault().ID);

        }

        public ValueMessageWrapper<int> GetSupplyBusinesPartner()
        {


            return ValueMessageWrapper.Create(_BusinessPartnerTypeRepository.Query(M => M.Code == "SupplyType").SingleOrDefault().ID);

        }

        public ValueMessageWrapper<int> GetDPBusinesPartner()
        {


            return  ValueMessageWrapper.Create(_BusinessPartnerTypeRepository.Query(M => M.Code == "DataProviderType").SingleOrDefault().ID);

        }
        #region Get
        public IEnumerable<PartyDto> GetAll()
        {
            var party = _partyRepository.GetAll();
            return party.Select(x => MapperHelper.Map<PartyDto>(x)).ToList();
        }

        public PartyListResultDto QueryByCriteria(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.PartyCriteria wcriteria)
        {

            PartyCriteria criteria = new PartyCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new PartyListResultDto();
            IEnumerable<Party> list = null;
            list = criteria.Page.HasValue ?
            _partyRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, true) :
            _partyRepository.Query(criteria.GetExpression());
            if(criteria.Page.HasValue && criteria.Type != Domain.Common.Model.Core.PartyType.All )
                result.TotalCount = _partyRepository.Query(criteria.GetExpression()).Count();

            if (list != null && !wcriteria.NotType)
            {
                foreach (var ot in list)
                {
                    if (ot is BusinessPartner || ot is DSPPartner || ot is DPPartner || ot is SSPPartner)

                    {
                        if (((BusinessPartner)ot).Type != null)
                            ot.TypeNameString = ((BusinessPartner)ot).Type.GetDescription();
                     

                    }

                    if (criteria.Type.HasValue && criteria.Type == Domain.Common.Model.Core.PartyType.All && string.IsNullOrEmpty(ot.TypeNameString))
                    {
                        if (ot is BusinessPartner)
                        {
                            ot.TypeNameString = "BusinessPartner";
                        }
                        else
                        {
                            ot.TypeNameString = "Account";

                        }
                    }


                }
            }

            var returnList = list.Select(x => MapperHelper.Map<PartyDto>(x)).ToList();

            result.Items = returnList;
            return result;
        }
        public PartyListResultDto QueryByCriteriaForDPPartner(ArabyAds.AdFalcon.Domain.Common.Repositories.Core.DPPartnerCriteria wcriteria)
        {
            DPPartnerCriteria criteria = new DPPartnerCriteria();
            criteria.CopyFromCommonToDomain(wcriteria);
            var result = new PartyListResultDto();
            IEnumerable<DPPartner> list = null;
            list =  _DPPartnerRepository.Query(M => M.IsExternalProvider == true && M.IsDeleted == false && (M.HasAccountWhite == true && M.AccountWhiteList.Any(C => C.Account.ID == Framework.OperationContext.Current.UserInfo<ArabyAds.Framework.UserInfo.IUserInfo>().AccountId))).OrderBy(M => M.Name);


           // _DPPartnerRepository.Query(criteria.GetExpression(), criteria.Page.Value - 1, criteria.Size, item => item.ID, true) :
           // _DPPartnerRepository.Query(criteria.GetExpression());

            //result.TotalCount = _DPPartnerRepository.Query(criteria.GetExpression()).Count();

            //if (list != null)
            //{
            //    foreach (var ot in list)
            //    {
            //        if (ot is BusinessPartner || ot is DSPPartner || ot is DPPartner || ot is SSPPartner)

            //        {
            //            if (((BusinessPartner)ot).Type != null)
            //                ot.TypeNameString = ((BusinessPartner)ot).Type.GetDescription();


            //        }
            //    }
            //}

            var returnList = list.Select(x => MapperHelper.Map<PartyDto>(x)).ToList();

            result.Items = returnList;
            return result;
        }

        public IList<PartyDto> GetAllExternalDPPartner(ValueMessageWrapper<int> campId)
        {
            DPPartnerCriteria criteria = new DPPartnerCriteria();

            criteria.Visible = true;
            var camp = _CampaignRepository.Get(campId.Value);
            var result = new PartyListResultDto();
            IEnumerable<DPPartner> list = null;
            if (camp != null && camp.Advertiser != null)
            {
                list =
              _DPPartnerRepository.Query(M => M.IsExternalProvider == true && M.IsDeleted == false && (M.HasAdvertiserBlock == false || M.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID)) && (M.HasAccountWhite == true && M.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID))).OrderBy(M => M.Name);

                // result.TotalCount = _DPPartnerRepository.Query(criteria.GetExpression()).Count();

                //if (list != null)
                //{
                //    foreach (var ot in list)
                //    {
                //        if (ot is BusinessPartner || ot is DSPPartner || ot is DPPartner || ot is SSPPartner)

                //        {
                //            if (((BusinessPartner)ot).Type != null)
                //                ot.TypeNameString = ((BusinessPartner)ot).Type.GetDescription();


                //        }
                //    }
                //}

                var returnList = list.Select(x => MapperHelper.Map<PartyDto>(x)).ToList();

                return returnList;
            }
            return new List<PartyDto>();
        }
        public IList<PartyDto> GetAllInternalDPPartner(ValueMessageWrapper<int> campId)
        {
            DPPartnerCriteria criteria = new DPPartnerCriteria();

            criteria.Visible = true;
            var camp = _CampaignRepository.Get(campId.Value);
            var result = new PartyListResultDto();
            IEnumerable<DPPartner> list = null;
            if (camp != null && camp.Advertiser != null)
            {
                list =
              _DPPartnerRepository.Query(M => M.IsExternalProvider == false && M.IsDeleted == false && (M.HasAdvertiserBlock == false || M.AdvertiserBlockList.Any(C => C.Advertiser.ID != camp.Advertiser.ID)) && (M.HasAccountWhite == true && M.AccountWhiteList.Any(C => C.Account.ID == camp.Account.ID))).OrderBy(M => M.Name);

                // result.TotalCount = _DPPartnerRepository.Query(criteria.GetExpression()).Count();

                //if (list != null)
                //{
                //    foreach (var ot in list)
                //    {
                //        if (ot is BusinessPartner || ot is DSPPartner || ot is DPPartner || ot is SSPPartner)

                //        {
                //            if (((BusinessPartner)ot).Type != null)
                //                ot.TypeNameString = ((BusinessPartner)ot).Type.GetDescription();


                //        }
                //    }
                //}

                var returnList = list.Select(x => MapperHelper.Map<PartyDto>(x)).ToList();

                return returnList;
            }
            return new List<PartyDto>();
        }
        public PartyDto GetExternalDPPartner(ValueMessageWrapper<int> Id)
        {
            var partItem = _DPPartnerRepository.Query(M => M.ID == Id.Value).SingleOrDefault();
            return MapperHelper.Map<PartyDto>(partItem);
        }
        public EmployeeDto GetEmployee(ValueMessageWrapper<int> id)
        {
            var item = _partyRepository.Get(id.Value);
            if (item != null && !item.IsDeleted)
            {
                var dto = MapperHelper.Map<EmployeeDto>(item);


                if (dto.AccountId > 0)
                    dto.AccountName = this._accountRepository.Get(dto.AccountId.Value).GetName();


                return dto;
            }
            else
            {
                throw new DataNotFoundException();
            }
        }

        public BusinessPartnerDto GetBusinessPartner(ValueMessageWrapper<int> id)
        {
            var item = _partyRepository.Get(id.Value);
            if (item != null && !item.IsDeleted)
            {
                var dto = MapperHelper.Map<BusinessPartnerDto>(item);

                if (dto.AccountId > 0)
                    dto.AccountName = this._accountRepository.Get(dto.AccountId.Value).GetName();

                if (dto.AppSiteId > 0)
                    dto.AppSiteName = this._AppSiteRepository.Get(dto.AppSiteId.Value).Name;


                if (dto.WhileIPs == null)
                {
                    dto.WhileIPs = new List<WhitleListIPDto>();

                }

                return dto;
            }
            else
            {
                throw new DataNotFoundException();
            }
        }
        public BusinessPartnerDto GetDPPartnerByAccount(ValueMessageWrapper<int> id)
        {
            var item = _DPPartnerRepository.Query(x => x.Account.ID == id.Value).FirstOrDefault();

            if (item != null && item is DPPartner)
            {
                var dto = MapperHelper.Map<BusinessPartnerDto>(item);

                if (dto.AccountId > 0)
                    dto.AccountName = this._accountRepository.Get(dto.AccountId.Value).GetName();

                if (dto.AppSiteId > 0)
                    dto.AppSiteName = this._AppSiteRepository.Get(dto.AppSiteId.Value).Name;

                return dto;
            }
            else
            {
                return null;
            }

        }
        #endregion
        #region Saving
        public ValueMessageWrapper<bool> DeleteParty(ValueMessageWrapper<int> Id)
        {
            if (Id.Value > 0)
            {

                var ob = this._partyRepository.Get(Id.Value);
                ob.IsDeleted = true;
                this._partyRepository.Save(ob);
                return ValueMessageWrapper.Create(true);
            }

            return ValueMessageWrapper.Create(false);

        }
        public ValueMessageWrapper<bool> DeleteParties(int[] Ids)
        {

            if (Ids != null)
            {
                foreach (var item in Ids.Select(id => _partyRepository.Get(id)))
                {
                    item.IsDeleted = true;
                    _partyRepository.Save(item);

                }
                return ValueMessageWrapper.Create(true);
            }

            return ValueMessageWrapper.Create(false);

        }
        private void UpdateItem(Party item, PartyDto saveDto)
        {
            if (item is Employee)
            {
                var emp_item = item as Employee;
                var emp_saveDto = saveDto as EmployeeDto;
                emp_item.JobPosition = _jobPositionRepository.Get(emp_saveDto.JobPositionId);
                if (emp_saveDto.AccountId.HasValue)
                    emp_item.Account = _accountRepository.Get(emp_saveDto.AccountId.Value);
                else
                    emp_item.Account = null;
            }
            if (item is BusinessPartner || item is SSPPartner || item is DSPPartner || item is DPPartner)
            {
                var bp_item = item as BusinessPartner;
                var bp_saveDto = saveDto as BusinessPartnerDto;
                int? documentId = (saveDto as BusinessPartnerDto).documentId;

                if (documentId.HasValue && documentId > 0)
                    (item as BusinessPartner).Icon = _documentRepository.Get((int)documentId);
                else
                    (item as BusinessPartner).Icon = null;


                if (bp_saveDto.AdvertiserList != null && bp_saveDto.AdvertiserList.Count > 0)
                {

                    foreach (var advId in bp_saveDto.AdvertiserList)
                    {
                        if (bp_item.AdvertiserBlockList.Where(M => M.Advertiser.ID == advId).SingleOrDefault() == null)
                        {
                            bp_item.AdvertiserBlockList.Add(new BusinessPartnerAdvertiserBlock { Partner = bp_item, Advertiser = new Advertiser { ID = advId } });
                        }
                    }

                    for (var i = 0; i < bp_item.AdvertiserBlockList.Count; i++)
                    {
                        if (bp_saveDto.AdvertiserList.Where(M => M == bp_item.AdvertiserBlockList[i].Advertiser.ID).SingleOrDefault() == 0)
                        {
                            bp_item.AdvertiserBlockList.RemoveAt(i);
                            if (i > 0)
                                i--;
                        }
                    }

                    if (bp_item.AdvertiserBlockList != null && bp_item.AdvertiserBlockList.Count == 0)
                    {  bp_item.AdvertiserBlockList.Clear();
                    bp_item.HasAdvertiserBlock = false;
                }

                else
                    bp_item.HasAdvertiserBlock = true;
            }
                else
                {
                    if (bp_item.AdvertiserBlockList != null)
                        bp_item.AdvertiserBlockList.Clear();
                    bp_item.HasAdvertiserBlock = false;
                }


                if (bp_saveDto.AccountList!= null && bp_saveDto.AccountList.Count > 0)
                {

                    foreach (var advId in bp_saveDto.AccountList)
                    {
                        if (bp_item.AccountWhiteList.Where(M => M.Account.ID == advId).SingleOrDefault() == null)
                        {
                            bp_item.AccountWhiteList.Add(new BusinessPartnerAccountWhite { Partner = bp_item, Account = new ArabyAds.AdFalcon.Domain.Model.Account.Account { ID = advId } });
                        }
                    }

                    for (var i = 0; i < bp_item.AccountWhiteList.Count; i++)
                    {
                        if (bp_saveDto.AccountList.Where(M => M == bp_item.AccountWhiteList[i].Account.ID).SingleOrDefault() == 0)
                        {
                            bp_item.AccountWhiteList.RemoveAt(i);
                            if(i>0)
                            i--;

                        }
                    }

                    if (bp_item.AccountWhiteList != null && bp_item.AccountWhiteList.Count == 0)
                    {
                        bp_item.AccountWhiteList.Clear();
                        bp_item.HasAccountWhite = false;
                    }

                    else
                        bp_item.HasAccountWhite = true;
                }
                else
                {
                    if (bp_item.AccountWhiteList != null)
                        bp_item.AccountWhiteList.Clear();
                    bp_item.HasAccountWhite = false;
                }

                if (bp_saveDto.BlockedDomains != null && !string.IsNullOrWhiteSpace(bp_saveDto.BlockedDomains) )
                {
                    var blockedURls=bp_saveDto.BlockedDomains.Split( new char[] { '\n' } );
                    foreach (var advId in blockedURls)
                    {
                        if (bp_item.DomainBlockList.Where(M => M.Domain.ToLower()== advId.Trim().ToLower()).SingleOrDefault() == null)
                        {
                            bp_item.DomainBlockList.Add(new BusinessPartnerDomainBlock { Partner = bp_item, Domain= advId.Trim() });
                        }
                    }

                    for (var i = 0; i < bp_item.DomainBlockList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(blockedURls.Where(M => M.Trim().ToLower() == bp_item.DomainBlockList[i].Domain.Trim().ToLower()).FirstOrDefault()))
                        {
                            bp_item.DomainBlockList.RemoveAt(i);
                            if (i > 0)
                                i--;

                        }
                    }

                    if (bp_item.DomainBlockList != null && bp_item.DomainBlockList.Count == 0)
                    {
                        bp_item.DomainBlockList.Clear();
                     
                    }

                    
                }
                else
                {
                    if (bp_item.DomainBlockList != null)
                        bp_item.DomainBlockList.Clear();
                   
                }
                if (bp_saveDto.WebCreativeFormatsList != null && bp_saveDto.WebCreativeFormatsList.Count > 0)
                {

                    foreach (var FormatId in bp_saveDto.WebCreativeFormatsList)
                    {
                        if (bp_item.WebCreativeFormatsList.Where(M => M.CreativeFormat.ID == FormatId).SingleOrDefault() == null)
                        {
                            bp_item.WebCreativeFormatsList.Add(new SSPPartnerSupportedCreativeFormats { Partner = bp_item, EnvironmentType = EnvironmentType.Web, CreativeFormat = new CreativeFormat { ID = FormatId } });

                        }
                    }

                    for (var i = 0; i < bp_item.WebCreativeFormatsList.Count; i++)
                    {
                        if (bp_saveDto.WebCreativeFormatsList.Where(M => M == bp_item.WebCreativeFormatsList[i].CreativeFormat.ID).SingleOrDefault() == 0)
                        {
                            bp_item.WebCreativeFormatsList.RemoveAt(i);

                        }
                    }

                    if (bp_item.WebCreativeFormatsList != null && bp_item.WebCreativeFormatsList.Count == 0)
                        bp_item.WebCreativeFormatsList.Clear();
                }
                else
                {
                    if (bp_item.WebCreativeFormatsList != null)
                        bp_item.WebCreativeFormatsList.Clear();

                }

                if (bp_saveDto.MobileCreativeFormatsList != null && bp_saveDto.MobileCreativeFormatsList.Count > 0)
                {

                    foreach (var FormatId in bp_saveDto.MobileCreativeFormatsList)
                    {
                        if (bp_item.MobileCreativeFormatsList.Where(M => M.CreativeFormat.ID == FormatId).SingleOrDefault() == null)
                        {
                            bp_item.MobileCreativeFormatsList.Add(new SSPPartnerSupportedCreativeFormats { Partner = bp_item, EnvironmentType = EnvironmentType.App, CreativeFormat = new CreativeFormat { ID = FormatId } });

                        }
                    }

                    for (var i = 0; i < bp_item.MobileCreativeFormatsList.Count; i++)
                    {
                        if (bp_saveDto.MobileCreativeFormatsList.Where(M => M == bp_item.MobileCreativeFormatsList[i].CreativeFormat.ID).SingleOrDefault() == 0)
                        {
                            bp_item.MobileCreativeFormatsList.RemoveAt(i);

                        }
                    }

                    if (bp_item.MobileCreativeFormatsList != null && bp_item.MobileCreativeFormatsList.Count == 0)
                        bp_item.MobileCreativeFormatsList.Clear();
                }
                else
                {
                    if (bp_item.MobileCreativeFormatsList != null)
                        bp_item.MobileCreativeFormatsList.Clear();
                }

                if (bp_saveDto.BusinessPartnerTypeId > 0)
                    bp_item.Type = new BusinessPartnerType { ID = bp_saveDto.BusinessPartnerTypeId };
                else
                    bp_item.Type = null;
                bp_item.Address = bp_saveDto.Address;
                bp_item.ContactPerson = bp_saveDto.ContactPerson;
                bp_item.Email = bp_saveDto.Email;
                bp_item.Visible = bp_saveDto.Visible;
                bp_item.Phone = bp_saveDto.Phone;
                //bp_item.BlockedDomains= bp_saveDto.BlockedDomains;
                if (bp_saveDto.AccountId.HasValue)
                    bp_item.Account = _accountRepository.Get(bp_saveDto.AccountId.Value);
                else
                    bp_item.Account = null;
                bp_item.Code = bp_saveDto.Code;

                if (item is SSPPartner)
                {
                    var ssp_item = item as SSPPartner;

                    if (bp_saveDto.AppSiteId.HasValue)
                        ssp_item.AppSite = new Domain.Model.AppSite.AppSite { ID = bp_saveDto.AppSiteId.Value };
                    else
                        ssp_item.AppSite = null;
                    ssp_item.DefaultSeatId = string.IsNullOrEmpty(bp_saveDto.DefaultSeatId) ? null : bp_saveDto.DefaultSeatId;
                    ssp_item.OpenRtbVersion = string.IsNullOrEmpty(bp_saveDto.OpenRtbVersion) ? null : bp_saveDto.OpenRtbVersion;
                    ssp_item.EncryptionKey = string.IsNullOrEmpty(bp_saveDto.EncryptionKey) ? null : bp_saveDto.EncryptionKey;

                    ssp_item.IntegrityKey = string.IsNullOrEmpty(bp_saveDto.IntegrityKey) ? null : bp_saveDto.IntegrityKey;
                    ssp_item.AuctionPriceMacroName = string.IsNullOrEmpty(bp_saveDto.AuctionPriceMacroName) ? string.Empty : bp_saveDto.AuctionPriceMacroName;

                    ssp_item.DoubleEncodedClickTrackerMacroName = string.IsNullOrEmpty(bp_saveDto.DoubleEncodedClickTrackerMacroName) ? null : bp_saveDto.DoubleEncodedClickTrackerMacroName;
                    ssp_item.ProvideImpressionTrackersMechanism = bp_saveDto.ProvideImpressionTrackersMechanism;

                    ssp_item.ClickTrackerMacroName = string.IsNullOrEmpty(bp_saveDto.ClickTrackerMacroName) ? null : bp_saveDto.ClickTrackerMacroName;

                    ssp_item.SupportMultipleClickTrackers = bp_saveDto.SupportMultipleClickTrackers;


                    ssp_item.NumberOfSupportedClickTrackersInNative = bp_saveDto.ClicksTrackers;


                    ssp_item.NumberOfSupportedImpressionTrackersInNative = bp_saveDto.ImpressionTrackers;

                    ssp_item.WhitelistIPs = string.IsNullOrEmpty(bp_saveDto.WhitelistIPs) ? null : bp_saveDto.WhitelistIPs;


                    ssp_item.AuctionPriceEncryptionKey = string.IsNullOrEmpty(bp_saveDto.AuctionPriceEncryptionKey) ? null : bp_saveDto.AuctionPriceEncryptionKey;
                    ssp_item.AuctionPriceIntegrityKey = string.IsNullOrEmpty(bp_saveDto.AuctionPriceIntegrityKey) ? null : bp_saveDto.AuctionPriceIntegrityKey;
                    ssp_item.AuctionPriceTestValue = string.IsNullOrEmpty(bp_saveDto.AuctionPriceTestValue) ? null : bp_saveDto.AuctionPriceTestValue;

                    if (!string.IsNullOrEmpty(bp_saveDto.AuctionPriceEncryptionAlgorithmId))
                        ssp_item.AuctionPriceEncryptionAlgorithmId = bp_saveDto.AuctionPriceEncryptionAlgorithmId.ToString();
                    else
                        ssp_item.AuctionPriceEncryptionAlgorithmId = null;

                    ssp_item.AuctionPricePricingUnitId = bp_saveDto.AuctionPricePricingUnitId.ToString();


                    ssp_item.TaggingAllowed = bp_saveDto.TaggingAllowed;

                    ssp_item.DeviceOSIdsIncludeValidUserId = bp_saveDto.DeviceOSIdsIncludeValidUserId;
                    ssp_item.ReportUnfilledRequests = bp_saveDto.ReportUnfilledRequests;
                    ssp_item.NumberOfSupportedImpressionTrackersInPartnerMechanism = bp_saveDto.NumberOfSupportedImpressionTrackersInPartnerMechanism;
                    ssp_item.NumberOfSupportedVastWrapperLevels = bp_saveDto.NumberOfSupportedVastWrapperLevels;

                    ssp_item.DisallowGeofenceLessThanRadius = bp_saveDto.DisallowGeofenceLessThanRadius;
                    ssp_item.GeofenceRadius = bp_saveDto.GeofenceRadius;
                    if (!ssp_item.DisallowGeofenceLessThanRadius) { ssp_item.GeofenceRadius = 0; }
                    if (ssp_item.GeofenceRadius == 0)
                    {
                        ssp_item.DisallowGeofenceLessThanRadius = false;
                    }
                    ssp_item.FingerPrintAllowed = bp_saveDto.FingerPrintAllowed;
                    ssp_item.AllowExchangeCreativeFormat = bp_saveDto.AllowExchangeCreativeFormat;

                    ssp_item.SupportWinNotice = bp_saveDto.SupportWinNotice;

                    if (!string.IsNullOrEmpty(bp_saveDto.insertedIps))
                    {
                        IList<string> ips = bp_saveDto.insertedIps.Split(',');

                        foreach (string Ip in ips)
                        {
                            SSPPartnerWhiteIP cc = new SSPPartnerWhiteIP()
                            {
                                SSPPartner = ssp_item,
                                IP = IpHelper.ConvertIPToBytes(Ip)
                            };
                            var existIP = ssp_item.WhileIPs.Where(M => M.IP == IpHelper.ConvertIPToBytes(Ip)).SingleOrDefault();

                            if (existIP == null)
                            {
                                ssp_item.WhileIPs.Add(cc);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(bp_saveDto.deletedIps))
                    {
                        IList<string> ips = bp_saveDto.deletedIps.Split(',');
                        foreach (string Ip in ips)
                        {
                            var existIP = ssp_item.WhileIPs.Where(M => string.Join(".", M.IP) == Ip).SingleOrDefault();
                            if (existIP != null)
                            {
                                ssp_item.WhileIPs.Remove(existIP);
                            }
                        }
                    }

                    if (ssp_item.WhileIPs != null && ssp_item.WhileIPs.Count == 0)
                        ssp_item.WhileIPs.Clear();

                }

                if (item is DSPPartner)
                {
                    var dsp_item = item as DSPPartner;

                    if (bp_saveDto.AppSiteId.HasValue)
                        dsp_item.AppSite = new Domain.Model.AppSite.AppSite { ID = bp_saveDto.AppSiteId.Value };
                    else
                        dsp_item.AppSite = null;

                }

                if (item is DPPartner)
                {
                    var dsp_item = item as DPPartner;

                        dsp_item.SiteProviderURL = bp_saveDto.SiteProviderURL;
                    dsp_item.IsExternalProvider = bp_saveDto.IsExternalProvider;
                    dsp_item.AdMarkupLogRequired = bp_saveDto.AdMarkupLogRequired;
                    dsp_item.AllowImpressionTrackers = bp_saveDto.AllowImpressionTrackers;
                    dsp_item.IsFTPEnabled = bp_saveDto.IsFTPEnabled;
                    dsp_item.FTPURL = bp_saveDto.FTPURL;

                }

            }

            item.Name = saveDto.Name;

        }
        private int SaveParty<TEntity>(PartyDto saveDto)
            where TEntity : Party
        {
            //TODO:Osaleh to user anther logic
            if (!saveDto.ID.HasValue)
                saveDto.ID = 0;
            var item = _partyRepository.Get(saveDto.ID.Value);
            if (item != null)
            {
                //update
                UpdateItem(item, saveDto);
            }
            else
            {
                //Insert
                item = MapperHelper.Map<TEntity>(saveDto);
                if (item is Employee)
                {
                    (item as Employee).JobPosition = _jobPositionRepository.Get((saveDto as EmployeeDto).JobPositionId);
                    if (saveDto.AccountId.HasValue)
                        (item as Employee).Account = _accountRepository.Get((saveDto as EmployeeDto).AccountId.Value);
                    else
                        (item as Employee).Account = null;
                }
                if (item is BusinessPartner || item is SSPPartner || item is DSPPartner || item is DPPartner)
                {


                    int? documentId = (saveDto as BusinessPartnerDto).documentId;

                    if (documentId.HasValue && documentId > 0)
                        (item as BusinessPartner).Icon = _documentRepository.Get((int)documentId);
                    else
                        (item as BusinessPartner).Icon = null;

                    var bp_item = item as BusinessPartner;

                    if ((saveDto as BusinessPartnerDto).AdvertiserList != null)
                    {
                        bp_item.AdvertiserBlockList = new List<BusinessPartnerAdvertiserBlock>();



                        foreach (var advId in (saveDto as BusinessPartnerDto).AdvertiserList)
                        {
                            bp_item.AdvertiserBlockList.Add(new BusinessPartnerAdvertiserBlock { Partner = bp_item, Advertiser = new Advertiser { ID = advId } });

                        }

                        if (bp_item.AdvertiserBlockList != null && bp_item.AdvertiserBlockList.Count == 0)
                        {   bp_item.AdvertiserBlockList.Clear();
                        bp_item.HasAdvertiserBlock = false;
                    }

                    else
                        bp_item.HasAdvertiserBlock = true;

                }



                    if ((saveDto as BusinessPartnerDto).BlockedDomains != null && !string.IsNullOrWhiteSpace((saveDto as BusinessPartnerDto).BlockedDomains))
                    {
                        bp_item.DomainBlockList = new List<BusinessPartnerDomainBlock>();

                       var listdomains= (saveDto as BusinessPartnerDto).BlockedDomains.Split(new char[] { '\n'});

                        foreach (var advId in listdomains)
                        {
                            if (bp_item.DomainBlockList.Where(M => M.Domain.ToLower() == advId.Trim().ToLower()).SingleOrDefault() == null)
                            {
                                bp_item.DomainBlockList.Add(new BusinessPartnerDomainBlock { Partner = bp_item, Domain = advId.Trim() });
                            }
                        }

                        if (bp_item.DomainBlockList != null && bp_item.DomainBlockList.Count == 0)
                        {
                            bp_item.DomainBlockList.Clear();
                          
                        }

                       

                    }
                    if ((saveDto as BusinessPartnerDto).AccountList != null)
                    {
                        bp_item.AccountWhiteList = new List<BusinessPartnerAccountWhite>();



                        foreach (var advId in (saveDto as BusinessPartnerDto).AccountList)
                        {
                            bp_item.AccountWhiteList.Add(new BusinessPartnerAccountWhite { Partner = bp_item, Account = new ArabyAds.AdFalcon.Domain.Model.Account.Account { ID = advId } });

                        }

                        if (bp_item.AccountWhiteList != null && bp_item.AccountWhiteList.Count == 0)
                        {  bp_item.AccountWhiteList.Clear();
                        bp_item.HasAccountWhite = false;
                    }

                    else
                        bp_item.HasAccountWhite = true;

                }

                    var bp_saveDto = saveDto as BusinessPartnerDto;
                    bp_item.Address = bp_saveDto.Address;
                    if (bp_saveDto.BusinessPartnerTypeId > 0)
                        bp_item.Type = new BusinessPartnerType { ID = bp_saveDto.BusinessPartnerTypeId };
                    else
                        bp_item.Type = null;
                    bp_item.ContactPerson = bp_saveDto.ContactPerson;

                    bp_item.Code = bp_saveDto.Code;
                    bp_item.Email = bp_saveDto.Email;
                    bp_item.Phone = bp_saveDto.Phone;
                    bp_item.Visible = bp_saveDto.Visible;
                    //bp_item.BlockedDomains = bp_saveDto.BlockedDomains;
                    if (saveDto.AccountId.HasValue)
                        (item as BusinessPartner).Account = _accountRepository.Get(bp_saveDto.AccountId.Value);
                    else
                        (item as BusinessPartner).Account = null;

                    if (item is SSPPartner)
                    {
                        var ssp_item = item as SSPPartner;

                        if (bp_saveDto.AppSiteId.HasValue)
                        {
                            ssp_item.AppSite = new Domain.Model.AppSite.AppSite { ID = bp_saveDto.AppSiteId.Value };
                            var appSite = _AppSiteRepository.Get(bp_saveDto.AppSiteId.Value);
                            appSite.IsContainer = true;
                        }
                        else
                            ssp_item.AppSite = null;

                        ssp_item.DefaultSeatId = string.IsNullOrEmpty(bp_saveDto.DefaultSeatId) ? null : bp_saveDto.DefaultSeatId;
                        ssp_item.OpenRtbVersion = string.IsNullOrEmpty(bp_saveDto.OpenRtbVersion) ? null : bp_saveDto.OpenRtbVersion;
                        ssp_item.EncryptionKey = string.IsNullOrEmpty(bp_saveDto.EncryptionKey) ? null : bp_saveDto.EncryptionKey;

                        ssp_item.IntegrityKey = string.IsNullOrEmpty(bp_saveDto.IntegrityKey) ? null : bp_saveDto.IntegrityKey;
                        ssp_item.AuctionPriceMacroName = string.IsNullOrEmpty(bp_saveDto.AuctionPriceMacroName) ? string.Empty : bp_saveDto.AuctionPriceMacroName;

                        ssp_item.ClickTrackerMacroName = string.IsNullOrEmpty(bp_saveDto.ClickTrackerMacroName) ? null : bp_saveDto.ClickTrackerMacroName;
                        ssp_item.SupportMultipleClickTrackers = bp_saveDto.SupportMultipleClickTrackers;
                        ssp_item.WhitelistIPs = string.IsNullOrEmpty(bp_saveDto.WhitelistIPs) ? null : bp_saveDto.WhitelistIPs;
                        ssp_item.NumberOfSupportedClickTrackersInNative = bp_saveDto.ClicksTrackers;
                        ssp_item.ProvideImpressionTrackersMechanism = bp_saveDto.ProvideImpressionTrackersMechanism;
                        ssp_item.DoubleEncodedClickTrackerMacroName = string.IsNullOrEmpty(bp_saveDto.DoubleEncodedClickTrackerMacroName) ? null : bp_saveDto.DoubleEncodedClickTrackerMacroName;
                        ssp_item.NumberOfSupportedImpressionTrackersInNative = bp_saveDto.ImpressionTrackers;

                        ssp_item.AuctionPriceEncryptionKey = string.IsNullOrEmpty(bp_saveDto.AuctionPriceEncryptionKey) ? null : bp_saveDto.AuctionPriceEncryptionKey;
                        ssp_item.AuctionPriceIntegrityKey = string.IsNullOrEmpty(bp_saveDto.AuctionPriceIntegrityKey) ? null : bp_saveDto.AuctionPriceIntegrityKey;
                        ssp_item.AuctionPriceTestValue = string.IsNullOrEmpty(bp_saveDto.AuctionPriceTestValue) ? null : bp_saveDto.AuctionPriceTestValue;

                        if (!string.IsNullOrEmpty(bp_saveDto.AuctionPriceEncryptionAlgorithmId))
                            ssp_item.AuctionPriceEncryptionAlgorithmId = bp_saveDto.AuctionPriceEncryptionAlgorithmId.ToString();
                        else
                            ssp_item.AuctionPriceEncryptionAlgorithmId = null;
                        ssp_item.AuctionPricePricingUnitId = bp_saveDto.AuctionPricePricingUnitId.ToString();
                        if ((saveDto as BusinessPartnerDto).MobileCreativeFormatsList != null)
                        {
                            bp_item.MobileCreativeFormatsList = new List<SSPPartnerSupportedCreativeFormats>();



                            foreach (var FormatId in (saveDto as BusinessPartnerDto).MobileCreativeFormatsList)
                            {
                                bp_item.MobileCreativeFormatsList.Add(new SSPPartnerSupportedCreativeFormats { Partner = bp_item, EnvironmentType = EnvironmentType.App, CreativeFormat = new CreativeFormat { ID = FormatId } });

                            }

                            if (bp_item.MobileCreativeFormatsList != null && bp_item.MobileCreativeFormatsList.Count == 0)
                                bp_item.MobileCreativeFormatsList.Clear();

                        }
                        if ((saveDto as BusinessPartnerDto).WebCreativeFormatsList != null)
                        {
                            bp_item.WebCreativeFormatsList = new List<SSPPartnerSupportedCreativeFormats>();



                            foreach (var FormatId in (saveDto as BusinessPartnerDto).WebCreativeFormatsList)
                            {
                                bp_item.WebCreativeFormatsList.Add(new SSPPartnerSupportedCreativeFormats { Partner = bp_item, EnvironmentType = EnvironmentType.Web, CreativeFormat = new CreativeFormat { ID = FormatId } });

                            }

                            if (bp_item.WebCreativeFormatsList != null && bp_item.WebCreativeFormatsList.Count == 0)
                                bp_item.WebCreativeFormatsList.Clear();

                        }
                        ssp_item.TaggingAllowed = bp_saveDto.TaggingAllowed;

                        ssp_item.DeviceOSIdsIncludeValidUserId = bp_saveDto.DeviceOSIdsIncludeValidUserId;
                        ssp_item.ReportUnfilledRequests = bp_saveDto.ReportUnfilledRequests;
                        ssp_item.NumberOfSupportedImpressionTrackersInPartnerMechanism = bp_saveDto.NumberOfSupportedImpressionTrackersInPartnerMechanism;
                        ssp_item.NumberOfSupportedVastWrapperLevels = bp_saveDto.NumberOfSupportedVastWrapperLevels;

                        ssp_item.DisallowGeofenceLessThanRadius = bp_saveDto.DisallowGeofenceLessThanRadius;
                        ssp_item.GeofenceRadius = bp_saveDto.GeofenceRadius;
                        if (!ssp_item.DisallowGeofenceLessThanRadius) { ssp_item.GeofenceRadius = 0; }
                        if (ssp_item.GeofenceRadius == 0)
                        {
                            ssp_item.DisallowGeofenceLessThanRadius = false;
                        }
                        ssp_item.SupportWinNotice = bp_saveDto.SupportWinNotice;
                        ssp_item.AllowExchangeCreativeFormat = bp_saveDto.AllowExchangeCreativeFormat;





                        if (ssp_item.WhileIPs == null)
                        {
                            ssp_item.WhileIPs = new List<SSPPartnerWhiteIP>();

                        }


                        if (!string.IsNullOrEmpty(bp_saveDto.insertedIps))
                        {
                            IList<string> ips = bp_saveDto.insertedIps.Split(',');

                            foreach (string Ip in ips)
                            {
                                SSPPartnerWhiteIP cc = new SSPPartnerWhiteIP()
                                {
                                    SSPPartner = ssp_item,
                                    IP = IpHelper.ConvertIPToBytes(Ip)
                                };
                                var existIP = ssp_item.WhileIPs.Where(M => M.IP == IpHelper.ConvertIPToBytes(Ip)).SingleOrDefault();

                                if (existIP == null)
                                {
                                    ssp_item.WhileIPs.Add(cc);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(bp_saveDto.deletedIps))
                        {
                            IList<string> ips = bp_saveDto.deletedIps.Split(',');

                            foreach (string Ip in ips)
                            {
                                var existIP = ssp_item.WhileIPs.Where(M => M.IP == IpHelper.ConvertIPToBytes(Ip)).SingleOrDefault();
                                if (existIP != null)
                                {
                                    ssp_item.WhileIPs.Remove(existIP);
                                }
                            }
                        }

                        if (ssp_item.WhileIPs != null && ssp_item.WhileIPs.Count == 0)
                            ssp_item.WhileIPs.Clear();
                    }

                    if (item is DSPPartner)
                    {
                        var dsp_item = item as DSPPartner;

                        if (bp_saveDto.AppSiteId.HasValue)
                            dsp_item.AppSite = new Domain.Model.AppSite.AppSite { ID = bp_saveDto.AppSiteId.Value };
                        else
                            dsp_item.AppSite = null;

                    }
                    if (item is DPPartner)
                    {
                        var dsp_item = item as DPPartner;
                        dsp_item.SiteProviderURL = bp_saveDto.SiteProviderURL;
                        dsp_item.IsExternalProvider = bp_saveDto.IsExternalProvider;
                        dsp_item.SiteProviderURL = bp_saveDto.SiteProviderURL;
                        dsp_item.IsFTPEnabled = bp_saveDto.IsFTPEnabled;
                        dsp_item.FTPURL = bp_saveDto.FTPURL;
                        dsp_item.AdMarkupLogRequired = bp_saveDto.AdMarkupLogRequired;
                        dsp_item.AllowImpressionTrackers = bp_saveDto.AllowImpressionTrackers;
                    }
                }
            }
            if ((item is BusinessPartner) && ((item as BusinessPartner).Account!=null) )
            {
                if (item is DSPPartner)
                {
                    var dsp_item = item as DSPPartner;


                    if (!((item as DSPPartner).Account.PrimaryUser.UserAccounts.Any(M => M.Account.AccountRole == ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DSP)))
                    {

                        var account = (item as DSPPartner).Account.Clone();
                        _accountRepository.Save(account);
                        (item as DSPPartner).Account = account;

                        (item as DSPPartner).Account.GoDSP();
                        (item as DSPPartner).Account.PrimaryUser.AttachAccount(account.ID,UserType.Normal);
                        var userOb = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>();
                        userOb.SwitchAccountSet = true;
                        ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>(userOb);
                    }
                    else
                    {
                        var accountobj = (item as DSPPartner).Account.PrimaryUser.UserAccounts.Where(M => M.Account.AccountRole == ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider).FirstOrDefault();

                        (item as DSPPartner).Account = accountobj.Account;

                        (item as DSPPartner).Account.GoDSP();
                    }

                }

                if (item is DPPartner)
                {
                    if (!((item as DPPartner).Account.PrimaryUser.UserAccounts.Any(M => M.Account.AccountRole == ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider)))
                    {
                        var account = (item as DPPartner).Account.Clone();
                        _accountRepository.Save(account);
                        (item as DPPartner).Account = account;

                        (item as DPPartner).Account.GoDataProvider();
                        (item as DPPartner).Account.PrimaryUser.AttachAccount(account.ID, UserType.Normal);
                        var userOb = ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>();
                        userOb.SwitchAccountSet = true;
                        ArabyAds.Framework.OperationContext.Current.UserInfo<ArabyAds.AdFalcon.Common.UserInfo.AdFalconUserInfo>(userOb);
                    }
                    else
                    {
                        var accountobj = (item as DPPartner).Account.PrimaryUser.UserAccounts.Where(M => M.Account.AccountRole == ArabyAds.AdFalcon.Domain.Common.Model.Account.AccountRole.DataProvider).FirstOrDefault();

                        (item as DPPartner).Account = accountobj.Account;

                        (item as DPPartner).Account.GoDataProvider();
                    }
                }
            }

            item.Validate();
            _partyRepository.Save(item);


            return item.ID;



        }
        public ValueMessageWrapper<int> SaveEmployee(EmployeeDto saveDto)
        {
            return ValueMessageWrapper<int>.Create(SaveParty<Employee>(saveDto));
        }

        public ValueMessageWrapper<int> SaveBusinessPartner(BusinessPartnerDto saveDto)
        {


            if (saveDto.BusinessPartnerTypeId > 0)
            {
                var type = this._BusinessPartnerTypeRepository.Get(saveDto.BusinessPartnerTypeId);
                if (type.Code == "DemandType")
                {
                    return ValueMessageWrapper.Create(SaveParty<DSPPartner>(saveDto));

                }
                if (type.Code == "SupplyType")
                {
                    return ValueMessageWrapper.Create(SaveParty<SSPPartner>(saveDto));

                }

                if (type.Code == "DataProviderType")
                {
                    var saveseq = true;
                    if (saveDto.ID.HasValue && saveDto.ID > 0)
                    {
                        saveseq = false;

                    }
                    else
                    {
                        var audianceseg = this.audianSegRep.Query(M => M.Provider.ID == saveDto.ID && M.Parent == null).SingleOrDefault();
                        if (audianceseg != null)
                        {
                            audianceseg.Name.SetValue(saveDto.Name, "en-US");
                            audianceseg.Name.SetValue(saveDto.Name, "ar-JO");
                            this.audianSegRep.Save(audianceseg);
                        }

                    }
                    var dpParten = SaveParty<DPPartner>(saveDto);
                    if (saveseq)
                    {
                        saveDto.ID = dpParten;

                        SaveAudianceSegment(saveDto);
                    }
                    return ValueMessageWrapper.Create(dpParten);


                }
                return ValueMessageWrapper.Create(SaveParty<BusinessPartner>(saveDto));
            }
            return ValueMessageWrapper.Create(SaveParty<BusinessPartner>(saveDto));
        }
        public bool DeleteEmployee(int Id)
        {
            if (Id > 0)
            {

                var ob = this._EmployeeRepository.Get(Id);
                ob.IsDeleted = true;
                this._EmployeeRepository.Save(ob);
                return true;
            }

            return false;
        }

        public bool DeleteBusinessPartner(int Id)
        {
            if (Id > 0)
            {

                var ob = this._BusinessPartnerRepository.Get(Id);
                ob.IsDeleted = true;
                this._BusinessPartnerRepository.Save(ob);
                return true;
            }

            return false;
        }

        private void SaveAudianceSegment(BusinessPartnerDto dto)
        {
            AudienceSegment obj = new AudienceSegment();
            obj.Name = new LocalizedString();
            obj.Name.GroupKey = "AudienceSegment";
            obj.Provider = new DPPartner
            {
                ID = dto.ID.Value
            };
            obj.Name.SetValue(dto.Name, "en-US");
            obj.Name.SetValue(dto.Name, "ar-JO");
            obj.Code = getUniqueCode();
            obj.OperatorSegmentCode = dto.Code;
            audianSegRep.Save(obj);
        }

        public int getUniqueCode()
        {
            var nhibernateSession = UnitOfWork.Current.OrmSession<ISession>();




            IQuery query = nhibernateSession.CreateSQLQuery("call CounterGetByName(:CounterName)");
            query.SetString("CounterName", "My Audience Segments");
            //query.SetInt32("YearId", Framework.Utilities.Environment.GetServerTime().Year);
            var count = query.UniqueResult();
            return Convert.ToInt32(count);

        }

       
        public IList<BusinessPartnerDto> GetDPPartnersFTP()
        {
            IList<BusinessPartnerDto> results = new List<BusinessPartnerDto>();
               var items = _DPPartnerRepository.Query(x => x.IsFTPEnabled==true&& x.IsDeleted==false).ToList();

            if (items != null )
            {
                foreach (var item in items)
                {
                    var dto = MapperHelper.Map<BusinessPartnerDto>(item);

                    results.Add( dto);
                }
                return results;
            }
            else
            {
                return null;
            }

        }
        public IList<ImpressionLogDto> GetImpressionLogsNotWrritten(ValueMessageWrapper<int> ProviderId)
        {
            ImpressionLogCriteria criteria = new ImpressionLogCriteria();
            criteria.DataProviderId = ProviderId.Value;
            var CurrentTime = Framework.Utilities.Environment.GetServerTime();
            var dateTo = Framework.Utilities.Environment.GetServerTime().AddDays(-1);
            var dateFrom = Framework.Utilities.Environment.GetServerTime().AddDays(-60);
            criteria.DataToInt= Convert.ToInt32(dateTo.ToString("yyyyMMdd"));
            criteria.DataFromInt =  Convert.ToInt32(dateFrom.ToString("yyyyMMdd"));
            IList<ImpressionLogDto> allpaths = new List<ImpressionLogDto>();
            var items= _ImpressionLogRepository.Query(criteria.GetExpression()).ToList(); ;
            if (items != null)
            {
                var finalResult = items.ToList();
                if (finalResult!=null)
                {
                   
                    IList<ImpressionLogDto> paths = new List<ImpressionLogDto>();
                    foreach (var fin in finalResult)
                    {
                        if (!fin.Written)
                            paths.Add(MapperHelper.Map<ImpressionLogDto>(fin));
                        else if (fin.LastUpdate >  dateTo )
                        paths.Add(MapperHelper.Map<ImpressionLogDto>(fin));
                    }
                    return paths;
                }
            }
            return allpaths;
        }

        public void SaveImpressionLogWrritten(ValueMessageWrapper<int> Id)
        {
           var impItem= _ImpressionLogRepository.Get(Id.Value);

            impItem.Written = true;
            _ImpressionLogRepository.Save(impItem);
          
        }
        #endregion
    }
}
