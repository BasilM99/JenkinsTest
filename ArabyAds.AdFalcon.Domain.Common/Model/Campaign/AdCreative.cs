﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.Framework.DataAnnotations;
using Noqoush.Framework.ExceptionHandling.Exceptions;
using System.Linq;
using Noqoush.Framework.DomainServices;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.Framework;
using Noqoush.AdFalcon.EventDTOs;
using Noqoush.Framework.DomainServices.EventBroker;

namespace Noqoush.AdFalcon.Domain.Model.Campaign
{
    public class AdCreative : AdBase<AdCreative, AdCreativeStatus>
    {

        protected IAdTypeRepository adTypeRepository = Framework.IoC.Instance.Resolve<IAdTypeRepository>();
        protected IAdCreativeRepository adCreativeRepository = Framework.IoC.Instance.Resolve<IAdCreativeRepository>();
        public AdCreative()
        {
            AdCreativeUnits = new List<AdCreativeUnit>();
            AdCustomParameters = new List<AdCustomParameter>();
        }

        public virtual IList<ClickTagTracker> ClickTags { get; set; }

        public virtual IList<ThirdPartyTracker> ThirdPartyTrackers { get; set; }

        public virtual AdExtension AdExtension { get; set; }
        public virtual ClickMethod ClickMethod { get; set; }
        public virtual bool? UpdatedbyPortal { get; set; }
        public virtual AdCreative Parent { get; set; }
        public virtual bool IsSecureCompliant { get; set; }
        public override string Name { get; set; }
        private string _NameLower;

        public virtual string NameLower { get { return _NameLower; } set { _NameLower = !string.IsNullOrEmpty(Name) ? Name.Trim().ToLower() : ""; } }
        //public virtual decimal? DataBid { get; set; }
        //public virtual decimal? MaxDataBid { get; set; }
        private static IList<string> StatusInfoPropTobeNotifyed = new List<string> { "Status" };
        public virtual string uId { get; set; }
        public virtual string AdText { get; set; }
        public virtual IList<AdCreativeUnit> AdCreativeUnits { get; set; }
        public virtual IList<AdCustomParameter> AdCustomParameters { get; set; }
        protected virtual decimal Bid { get; set; }

        public virtual decimal DiscountedBid
        {
            get { return this.Group.Campaign.GetDiscountedBid(this.Group, this); }
        }
        public virtual DeviceTypeEnum CretiveUnitDeviceType { get; set; }
        public virtual AdCreativeStatus PausedStatus { get; set; }
        public virtual AdGroup Group { get; set; }

        public virtual Language Language { get; set; }


        //private AdType _adtype;
        public virtual AdType Type
        {
            get;
            //{
            //    if (_adtype == null)
            //        _adtype = adTypeRepository.Get((int)TypeId);
            //    return _adtype;
            //}
            set;
        }
        public virtual AdTypeIds TypeId { get; set; }
        public virtual AdSubTypes? AdSubType { get; set; }


        public virtual AdType TypeForPortal { get; set; }
        public virtual AdSubTypes? AdSubTypeForPortal { get; set; }
        public virtual EnvironmentType? EnvironmentType { get; set; }
        public virtual OrientationType? OrientationType { get; set; }
        public virtual AdActionValue ActionValue { get; set; }
        public virtual IList<AppSiteAdQueue> AppSiteAdQueues { get; set; }
        public override AdCreativeStatus Status { get; set; }
        public virtual string DomainURL { get; set; }
        public virtual Keyword Keyword { get; set; }
        public virtual bool PublishEventFromAdGroup { get; set; }
        public virtual string GetAdSubTypDescription(string AdSubType)
        {
            if (!string.IsNullOrEmpty(AdSubType))
            {
                Enum enumTobe = (Enum)Enum.Parse(typeof(AdSubTypes), AdSubType);
                if (Convert.ToInt32(enumTobe) > 0)
                {
                    return enumTobe.ToText();
                }
                return string.Empty;

            }
            else
            {
                return string.Empty;

            }
        }

        public virtual decimal GetReadableBid()
        {
            if (Group == null || Group.CostModelWrapper == null)
                return this.Bid;

            return this.Bid * Group.CostModelWrapper.Factor;
        }
        public virtual decimal GetBid()
        {
            return this.Bid;
        }
        public virtual void SetAdCreativeBid(decimal value, int factor)
        {
            this.Bid = value / factor;

            if (this.AppSiteAdQueues != null)
            {
                UpdateAppSiteAdQueueBid(this.Bid);
            }
        }

        #region AdCreative Units

        public virtual AdCreativeUnit GetCreativeUnit(int creativeUnitId)
        {
            var adCreativeUnit = AdCreativeUnits.FirstOrDefault(item => item.CreativeUnit.ID == creativeUnitId);
            return adCreativeUnit;
        }
        public virtual IList<AdCreativeUnit> GetCreativeUnits()
        {
            return AdCreativeUnits;
        }
        public virtual void ClearUnusedBanners()
        {
            var unusedItems = AdCreativeUnits.Where(item => string.IsNullOrWhiteSpace(item.Content)).ToList();
            foreach (var unusedItem in unusedItems)
            {
                // unusedItem.Trackers.Clear();
                // AdCreativeUnits.Remove(unusedItem);
                RemoveAdCreativeUnit(unusedItem.ID);
            }
        }
        public virtual void RemoveAdCreativeUnit(int creativeUnitId)
        {
            var item = GetCreativeUnit(creativeUnitId);
            //item.Trackers.Clear();
            item.Delete();
            //AdCreativeUnits.Remove(item);
        }

        public virtual void AddCreativeUnit(AdCreativeUnit addCreativeUnit)
        {
            addCreativeUnit.AdCreative = this;
            AdCreativeUnits.Add(addCreativeUnit);
        }

        public virtual void AddAdCustomParameter(string name, string value)
        {

            AdCustomParameter adCustomParameter = GetAdCustomParam(name);
            if (adCustomParameter == null)
            {
                adCustomParameter = new AdCustomParameter();
                adCustomParameter.Name = name;
                adCustomParameter.Value = value;
                adCustomParameter.AdCreative = this;
                adCustomParameter.IsMandatory = true;
                this.AdCustomParameters.Add(adCustomParameter);
            }
        }
        public virtual void AddAdCustomParameter(string name, string value,bool IsRequired)
        {

            AdCustomParameter adCustomParameter = GetAdCustomParam(name);
            if (adCustomParameter == null)
            {
                adCustomParameter = new AdCustomParameter();
                adCustomParameter.Name = name;
                adCustomParameter.Value = value;
                adCustomParameter.AdCreative = this;
                adCustomParameter.IsMandatory = IsRequired;

                this.AdCustomParameters.Add(adCustomParameter);
            }
        }
        public virtual void AddAdCustomParameterNotMandatory(string name, string value)
        {

            AdCustomParameter adCustomParameter = GetAdCustomParam(name);
            if (adCustomParameter == null)
            {
                adCustomParameter = new AdCustomParameter();
                adCustomParameter.Name = name;
                adCustomParameter.Value = value;
                adCustomParameter.AdCreative = this;
                adCustomParameter.IsMandatory = false;
                this.AdCustomParameters.Add(adCustomParameter);
            }
        }
        public virtual void RemoveAdCustomParameter(AdCustomParameter adCustomParameter)
        {
            adCustomParameter.IsDeleted = true;
        }

        public virtual AdCustomParameter GetAdCustomParam(string name)
        {
            return this.AdCustomParameters.Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }
        #endregion
        public override bool Resume()
        {
            if (this.PausedStatus != null)
            {
                this.Status = this.PausedStatus;
                this.PausedStatus = null;
            }
            this.UpdatedbyPortal = true;
            return base.Resume();
        }
        public override bool Pause()
        {
            if (Status.ID != AdCreativeStatus.Paused.ID)
            {
                this.PausedStatus = this.Status;
                if (this.Status.ID == AdCreativeStatus.ActiveAdServer.ID)
                    this.PausedStatus = AdCreativeStatus.Active;
               this.Status = AdCreativeStatus.Paused;
               // if(this.PublishEventFromAdGroup==false)
                //PublishAdCreatPauseEventForKafka();
            }
            this.UpdatedbyPortal = true;
            return base.Pause();

        }
        public virtual bool PublishAdStatusInforForKafka(EntityEventData args)
        {

            if (!(args.DirtyProperties != null && args.DirtyProperties.Length > 0 && this.ID > 0))
            {
                return false;
            }
            var isDirty = false;

            foreach (var item in StatusInfoPropTobeNotifyed)
            {
                var index = Array.IndexOf(args.PropertyNames, item);
                isDirty = args.DirtyProperties.Contains(index);
                if (isDirty)
                {
                    break;


                }

            }
            if (isDirty)
            {
                PauseAdEvent adgroupChanged = new PauseAdEvent();

                var newadgroup = adCreativeRepository.Get(this.ID);
          
                foreach (var item in StatusInfoPropTobeNotifyed)
                {
                   var index = Array.IndexOf(args.PropertyNames, item);

                    if (item == "Status")
                    {
                        var status = (AdCreativeStatus)(args.State[index]);
                        if (status != null && status.ID == AdCreativeStatus.Paused.ID)
                            return true;


                    }

                }
               
            }

            return false;
        }
        public virtual void PublishAdCreatPauseEventForKafka()
        {



            if (Configuration.KafkaEnabled)
                Configuration.KafkaEventPublisher.Publish(new PauseAdEvent { AdIds = new List<int> { this.ID }, });
        }
        public virtual void Approve()
        {
            if (Status.ID == AdCreativeStatus.Paused.ID)
            {
                this.PausedStatus = AdCreativeStatus.Active;
            }
            else
            {
                this.Status = AdCreativeStatus.Active;
            }
            if (AppSiteAdQueues != null)
            {
                foreach (var appSiteAdQueue in AppSiteAdQueues)
                {
                    appSiteAdQueue.Bid = -1 * Bid;
                }
            }

            this.UpdatedbyPortal = true;
        }

        protected virtual void UploadSnapshots()
        {
            var baseDirectory = Configuration.FtpBaseDirectory;
            var cdnBaseUrl = Configuration.CdnBaseUrl;

            string temp = string.Empty;
            var subFolder = string.Empty;
            if (!string.IsNullOrWhiteSpace(Group.Campaign.FolderURL))
            {
                subFolder = Group.Campaign.FolderURL;
            }
            else
            {
                //we need to create folder for it
                subFolder = Framework.Utilities.Environment.GetServerTime().ToString("yyyyMMdd");
                temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                //create folder fo the current date
                Framework.Utilities.FtpHelper.CreateDirectory(temp);
                //create folder for current Campaign
                var isFolderCreated = false;
                while (!isFolderCreated)
                {
                    temp = string.Format("{0}/{1}", baseDirectory, subFolder);
                    var r = RandomNumber(1, 100000);
                    temp = string.Format("{0}/{1}", temp, r);
                    if (!Framework.Utilities.FtpHelper.DirectoryExists(temp))
                    {
                        Framework.Utilities.FtpHelper.CreateDirectory(temp);
                        isFolderCreated = true;
                        subFolder += "/" + r;
                        Group.Campaign.FolderURL = subFolder;
                    }
                }
            }
            if (!Framework.Utilities.FtpHelper.DirectoryExists((baseDirectory + "/" + subFolder)))
            {
                ApplicationContext.Instance.Logger.Info(string.Format("SubFolder:{0}", (baseDirectory + "/" + subFolder)));
                Framework.Utilities.FtpHelper.CreateDirectory((baseDirectory + "/" + subFolder));
            }
            // create folder for current ad if not found
            subFolder = string.Format("{0}/{1}", subFolder, uId);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);

            Framework.Utilities.FtpHelper.CreateDirectory(temp);

            subFolder = string.Format("{0}/Snapshots", subFolder);
            temp = string.Format("{0}/{1}", baseDirectory, subFolder);
            Framework.Utilities.FtpHelper.CreateDirectory(temp);

            var directory = string.Format("{0}/{1}", baseDirectory, subFolder);
            var cdnUrl = string.Format("{0}/{1}", cdnBaseUrl, subFolder);

            var duplicates = AdCreativeUnits.Where(p => p.SnapshotDocument != null).GroupBy(s => s.SnapshotDocument.Name.ToLower()).SelectMany(grp => grp.Skip(1));
            foreach (var adCreativeUnit in duplicates)
            {
                adCreativeUnit.SnapshotDocument.NewName = string.Format("{0}_{1}_{2}", adCreativeUnit.SnapshotDocument.GetNameWithNoExtension(),
                                                             adCreativeUnit.CreativeUnit.Width,
                                                             adCreativeUnit.CreativeUnit.Height);
            }

            foreach (var adCreativeUnit in AdCreativeUnits.Where(p => p.SnapshotDocument != null))
            {
                if (!adCreativeUnit.KeepShapshot)
                {
                    string url = adCreativeUnit.SnapshotDocument.ftpUpload(directory, cdnUrl);

                    adCreativeUnit.SnapshotUrl = url;
                    adCreativeUnit.KeepShapshot = true;
                }
            }
        }

        public virtual void UpdateAppSiteAdQueueType(bool include)
        {
            foreach (var appSiteAdQueue in AppSiteAdQueues)
            {
                appSiteAdQueue.Include = include ? Include.Include : Include.Exclude;
            }
        }
        public virtual void UpdateAppSiteAdQueueBid(decimal bid)
        {
            foreach (var appSiteAdQueue in AppSiteAdQueues)
            {
                appSiteAdQueue.Bid = -1 * bid;
            }
        }

        public virtual void AddAppSiteAdQueue(AppSite.AppSite appSite, bool include = true)
        {
            Include Include = include ? Include.Include : Include.Exclude;
            if (AppSiteAdQueues.Any(x => x.AppSite.ID == appSite.ID) == false)
            {
                AppSiteAdQueues.Add(new AppSiteAdQueue() { Ad = this, AppSite = appSite, Bid = -1 * Bid, Include = include ? Include.Include : Include.Exclude });
            }

        }
        public virtual void RemoveAppSiteAdQueue(AppSite.AppSite appSite)
        {
            var item = AppSiteAdQueues.FirstOrDefault(x => x.AppSite.ID == appSite.ID);
            if (item != null)
            {
                //item.Ad = null;
                AppSiteAdQueues.Remove(item);
            }
        }
        public virtual AppSiteAdQueue RemoveAppSiteAdQueueAndReturn(AppSite.AppSite appSite)
        {
            var item = AppSiteAdQueues.FirstOrDefault(x => x.AppSite.ID == appSite.ID);
            if (item != null)
            {
                //item.Ad = null;
                AppSiteAdQueues.Remove(item);
                return item;
            }
            return null;
        }

        public virtual bool ExistAppSiteAdQueue(AppSite.AppSite appSite)
        {
            var item = AppSiteAdQueues.FirstOrDefault(x => x.AppSite.ID == appSite.ID);
            if (item != null)
            {
                return true;
            }
            return false;
        }
        public virtual void ClearAppSiteAdQueue()
        {
            if (AppSiteAdQueues == null)
            {
                AppSiteAdQueues = new List<AppSiteAdQueue>();
            }
            //var temp = AppSiteAdQueues.ToList();
            //foreach (var appSiteAdQueue in temp)
            //{
            //    appSiteAdQueue.Ad = null;
            //    appSiteAdQueue.AppSite = null;
            //}
            AppSiteAdQueues.Clear();
        }
        public virtual void Disapproved()
        {
            if (Status.ID == AdCreativeStatus.Paused.ID)
            {
                this.PausedStatus = AdCreativeStatus.DisApproved;
            }
            else
            {
                this.Status = AdCreativeStatus.DisApproved;
            }
            this.UpdatedbyPortal = true;
        }
        public override bool Delete()
        {
            this.IsDeleted = true;

            foreach (var item in GetCreativeUnits())
            {
                item.Delete();
            }
            this.Pause();

                
            this.UpdatedbyPortal = true;
            return base.Delete();
        }
        public virtual bool IsValid { get; set; }
        public virtual void Validate(bool statusCheck = false)
        {
            IsValid = false;
            if (statusCheck)
            {
                //create business Exception to hold error data list 
                var error = new BusinessException();
                if (Group.CostModelWrapper != null && (Bid * Group.CostModelWrapper.Factor) < Group.GetReadableBid())
                {
                    if (!Domain.Configuration.IsAdminOnly)
                    {
                        error.Errors.Add(new ErrorData { ID = "MinBidErrMsg" });
                    }
                }
                //TODO: Osaleh to add the validation the Tile Images and AdCreativeUnits
                if (error.Errors.Count > 0)
                {
                    IsValid = false;
                    throw (error);
                }
                IsValid = DataAnnotationsValidator.TryValidate(this);
            }
            IsValid = true;
        }

        public virtual void SetWrapperContent(string content)
        {
          
            if (!string.IsNullOrWhiteSpace(content))
            {
                if (this.AdExtension == null)
                {
                    this.AdExtension = new AdExtension();
                    this.AdExtension.Creative = this;
                }
                this.AdExtension.WrapperContent = content;
            }
            else
            {
                if (this.AdExtension!=null)
                {
                    this.AdExtension.WrapperContent =null;
                }
            
            }
         
        }

        public virtual string GetWrapperContent()
        {

            
                if (this.AdExtension != null)
                {
                 return     this.AdExtension.WrapperContent ; 
                }

            return string.Empty;
          

        }
        //TODO:Osaleh to move this code to framework
        protected virtual string RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max).ToString();
        }

        public virtual AdCreative Clone(AdGroup adGroup)
        {
            var cloneObj = new AdCreative();
            cloneObj.Group = adGroup;
            cloneObj.Language = this.Language == null ? null : this.Language;

            cloneObj.EnableEventsPostback = EnableEventsPostback;
            cloneObj.VerifyTargetingCriteria = VerifyTargetingCriteria;
            cloneObj.VerifyDailyBudget = VerifyDailyBudget;

            cloneObj.VerifyCampaignStartAndEndDate = VerifyCampaignStartAndEndDate;
            cloneObj.ValidateRequestDeviceAndLocationData = ValidateRequestDeviceAndLocationData;
            cloneObj.VerifyPrerequisiteEvents = VerifyPrerequisiteEvents;
            cloneObj.UpdateEventsFrequency = UpdateEventsFrequency;
            cloneObj.UpdateTags = UpdateTags;
            cloneObj.VerifyEventsFrequency = VerifyEventsFrequency;
            cloneObj.ClickTags = this.ClickTags;
            if (this.AdExtension != null)
                cloneObj.AdExtension = this.AdExtension.Clone();
            
            return cloneObj;
        }
        public override AdCreative Clone()
        {
            return Clone(this.Group);
        }

        public virtual T Clone<T>() where T : AdCreative, new()
        {
            T cloneObj = new T();

            cloneObj.Group = Group;
            cloneObj.Language = this.Language == null ? null : this.Language;

            cloneObj.EnableEventsPostback = EnableEventsPostback;
            cloneObj.VerifyTargetingCriteria = VerifyTargetingCriteria;
            cloneObj.VerifyDailyBudget = VerifyDailyBudget;

            cloneObj.VerifyCampaignStartAndEndDate = VerifyCampaignStartAndEndDate;
            cloneObj.ValidateRequestDeviceAndLocationData = ValidateRequestDeviceAndLocationData;

            cloneObj.UpdateEventsFrequency = UpdateEventsFrequency;
            cloneObj.UpdateTags = UpdateTags;
            cloneObj.VerifyEventsFrequency = VerifyEventsFrequency;
            cloneObj.ActionValue = this.ActionValue == null ? null : this.ActionValue.Copy();
            cloneObj.AdText = this.AdText;
            cloneObj.UpdatedbyPortal = this.UpdatedbyPortal;
            cloneObj.Bid = this.Bid;
            //cloneObj.DataBid = this.DataBid;
            //cloneObj.MaxDataBid = this.MaxDataBid;
            cloneObj.IsSecureCompliant = this.IsSecureCompliant;
            cloneObj.CreationDate = Framework.Utilities.Environment.GetServerTime();
            cloneObj.Name = this.Name;
            cloneObj.Type = this.Type;
            cloneObj.AdSubType = this.AdSubType;

            cloneObj.TypeForPortal = this.TypeForPortal;
            cloneObj.AdSubTypeForPortal = this.AdSubTypeForPortal;
            cloneObj.uId = Guid.NewGuid().ToString();
            cloneObj.Status = AdCreativeStatus.Submitted;
            cloneObj.CretiveUnitDeviceType = this.CretiveUnitDeviceType;
            cloneObj.AppSiteAdQueues = new List<AppSiteAdQueue>();
            cloneObj.IsDeleted = this.IsDeleted;
            cloneObj.AdCreativeUnits = new List<AdCreativeUnit>();
            cloneObj.EnvironmentType = this.EnvironmentType;
            cloneObj.OrientationType = this.OrientationType;
            cloneObj.EnableEventsPostback = this.EnableEventsPostback;
            cloneObj.VerifyTargetingCriteria = this.VerifyTargetingCriteria;
            cloneObj.VerifyDailyBudget = this.VerifyDailyBudget;
            cloneObj.VerifyPrerequisiteEvents = this.VerifyPrerequisiteEvents;


            cloneObj.VerifyCampaignStartAndEndDate = this.VerifyCampaignStartAndEndDate;
            cloneObj.ValidateRequestDeviceAndLocationData = this.ValidateRequestDeviceAndLocationData;

            cloneObj.UpdateEventsFrequency = this.UpdateEventsFrequency;
            cloneObj.UpdateTags = this.UpdateTags;
            cloneObj.VerifyEventsFrequency = this.VerifyEventsFrequency;
            if (this.AdExtension != null)
                cloneObj.AdExtension = this.AdExtension.Clone();

            if (cloneObj.ActionValue != null)
            {
                cloneObj.ActionValue.AdCreative = cloneObj;
            }

            return (T)cloneObj;

        }
        public override string ToString()
        {
            return Name;
        }

        public virtual bool EnableEventsPostback { get; set; }
        public virtual bool VerifyTargetingCriteria { get; set; }

        public virtual bool VerifyDailyBudget { get; set; }
        public virtual bool VerifyCampaignStartAndEndDate { get; set; }
        public virtual bool UpdateEventsFrequency { get; set; }
        public virtual bool VerifyPrerequisiteEvents { get; set; }

        public virtual bool UpdateTags { get; set; }
        public virtual bool VerifyEventsFrequency { get; set; }

        public virtual bool ValidateRequestDeviceAndLocationData { get; set; }
    }

    public class AdExtension : IEntity<int>
    {
        public virtual int ID { get; set; }
        public virtual bool IsDeleted { get; set; }

        public virtual AdCreative Creative { get; set; }

     
        public virtual string WrapperContent { get; set; }

        public virtual string GetDescription()
        {
            return string.Empty;
        }
        public virtual AdExtension Clone()
        {

            return new AdExtension { Creative = this.Creative, WrapperContent = this.WrapperContent, IsDeleted = this.IsDeleted };
        }

    }
}

