using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Domain.Model.Account;
using Noqoush.AdFalcon.Domain.Model.Account.Fund;
using Noqoush.AdFalcon.Domain.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Model.AppSite;
using Noqoush.AdFalcon.Domain.Model.Campaign;
using Noqoush.AdFalcon.Domain.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Model.Core;
using Noqoush.AdFalcon.Domain.Repositories;
using Noqoush.AdFalcon.Domain.Repositories.Campaign;
using Noqoush.AdFalcon.Domain.Repositories.Campaign.Creative;
using Noqoush.AdFalcon.Domain.Repositories.Core;
using Noqoush.Framework;
using Noqoush.Framework.ConfigurationSetting;
using Noqoush.Framework.DomainServices;
using Noqoush.Framework.DomainServices.AuditTrial;
using Noqoush.Framework.DomainServices.EventBroker;
using Noqoush.Framework.DomainServices.Localization;
using Noqoush.Framework.DomainServices.Localization.Repositories;
using Noqoush.Framework.EventBroker;
using Noqoush.Framework.Persistence;
using Noqoush.Framework.Resources;
using Noqoush.Framework.Utilities.EmailsSender;
using NHibernate.Transform;
using Noqoush.AdFalcon.Domain.Repositories.Account.PMP;

using Noqoush.AdFalcon.Domain.Common.Model.Account;

using Noqoush.AdFalcon.Domain.Common.Model.Account.Payment;
using Noqoush.AdFalcon.Domain.Common.Model.AppSite;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Objective;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Targeting;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign.Targeting.Device;
using Noqoush.AdFalcon.Domain.Common.Model.Core;

namespace Noqoush.AdFalcon.Services.Utility
{
    public static class EventNames
    {
        public const string close_fund_transaction = "Close Fund Transaction";
        public const string Save_Campaign = "Campaign Save";
        public const string Save_ReportScheduler = "ReportScheduler Save";
        public const string Save_Targeting = "Save Targeting";
        public const string Save_Ad = "Save Ad";

        public const string LookUp_Save = "LookUp Save";

        public const string Delete_Campaign = "Delete Campaign";
        public const string Run_Campaign = "Run Campaign";
        public const string Pause_Campaign = "Pause Campaign";

        public const string Delete_AdGroup = "Delete AdGroup";
        public const string Run_AdGroup = "Run AdGroup";
        public const string Pause_AdGroup = "Pause AdGroup";

        public const string Delete_Ad = "Delete Ad";
        public const string Run_Ad = "Run Ad";
        public const string Pause_Ad = "Pause Ad";

        public const string Save_App = "Save App";
        public const string Delete_App = "Delete App";
        public const string AppSiteApprove = "AppSiteApprove";

        public const string Register_User = "Register User";

        public const string Update_Bank_Account_Info = "Update Bank Account Info";

        public const string Copy_Campaign = "Copy Campaign";
        public const string Copy_AdGroup = "Copy AdGroup";
        public const string Copy_Ad = "Copy Ad";
        public const string Add_Payment = "Add Payment";
        public const string Add_Fund = "Add Fund by Admin";
        public const string Account_DSP_Request = "Account DSP Request";
    }

    public class EmailSender : Framework.EventBroker.ISubscriberHandler
    {
        protected const string OrderedListTemplate = "<ol>{0}</ol>";
        protected const string OrderedListItemTemplate = "<li>{0}</li>";
        protected const string emailSubject = "EmailSubject{0}";
        protected const string _eventArgsMissingForEvent = "Event args Missing for '{0}' Event";
        protected IUserRepository userRepository = Framework.IoC.Instance.Resolve<IUserRepository>();
        protected IConfigurationManager config = Framework.IoC.Instance.Resolve<Framework.ConfigurationSetting.IConfigurationManager>();
        protected IAccountRepository accountRepository = Framework.IoC.Instance.Resolve<IAccountRepository>();
        protected IAccountFundTransHistoryRepository fundTransRepository = Framework.IoC.Instance.Resolve<IAccountFundTransHistoryRepository>();
        protected ICampaignRepository campaignRepository = Framework.IoC.Instance.Resolve<ICampaignRepository>();
        protected ITargetingBaseRepository targetingBaseRepository = Framework.IoC.Instance.Resolve<ITargetingBaseRepository>();
        protected ILocalizedStringRepository localizedStringRepository = Framework.IoC.Instance.Resolve<ILocalizedStringRepository>();
        protected IAppSiteRepository appSiteRepository = Framework.IoC.Instance.Resolve<IAppSiteRepository>();
        protected IPMPDealRepository PMPDealRepository = Framework.IoC.Instance.Resolve<IPMPDealRepository>();
        protected ILanguageRepository LanguageRepository = Framework.IoC.Instance.Resolve<ILanguageRepository>();
        protected IKeyWordRepository keyWordRepository = Framework.IoC.Instance.Resolve<IKeyWordRepository>();
        protected IAdvertiserRepository AdvertiserRepository = Framework.IoC.Instance.Resolve<IAdvertiserRepository>();
        protected IPlatformRepository platformRepository = Framework.IoC.Instance.Resolve<IPlatformRepository>();
        protected IOperatorRepository operatorRepository = Framework.IoC.Instance.Resolve<IOperatorRepository>();
        protected ITargetingTypeRepository targetingTypeRepository = Framework.IoC.Instance.Resolve<ITargetingTypeRepository>();
        protected ILocationRepository locationRepository = Framework.IoC.Instance.Resolve<ILocationRepository>();
        protected IManufacturerRepository manufacturerRepository = Framework.IoC.Instance.Resolve<IManufacturerRepository>();
        protected IDeviceRepository deviceRepository = Framework.IoC.Instance.Resolve<IDeviceRepository>();
        protected IDeviceCapabilityRepository deviceCapabilityRepository = Framework.IoC.Instance.Resolve<IDeviceCapabilityRepository>();
        protected IAdCreativeRepository adCreativeRepository = Framework.IoC.Instance.Resolve<IAdCreativeRepository>();
        protected IGenderRepository genderRepository = Framework.IoC.Instance.Resolve<IGenderRepository>();
        protected IAgeGroupRepository ageGroupRepository = Framework.IoC.Instance.Resolve<IAgeGroupRepository>();
        protected ITileImageRepository tileImageRepository = Framework.IoC.Instance.Resolve<ITileImageRepository>();
        protected IReportSchedulerRepository ReportSchedulerRepository = Framework.IoC.Instance.Resolve<IReportSchedulerRepository>();
        protected ICreativeVendorRepository CreativeVendorRepository = Framework.IoC.Instance.Resolve<ICreativeVendorRepository>();
        protected Account _CurrentAccount
        {
            get
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>() != null && OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.HasValue)
                    return accountRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value);
                else
                    return null;
            }
        }


        private ISession _NhibernateSession
        {
            get
            {
                return UnitOfWork.Current.OrmSession<ISession>();
            }
        }

        private List<EventEmails> _eventEmails;

        private List<EventEmails> _EventsEmails
        {
            get
            {
                if (_eventEmails == null)
                {
                    _eventEmails = GetEventsEmails();
                }

                return _eventEmails;
            }
        }

        public EmailSender()
        {

        }

        #region Helpers

        private static readonly string[] IgnoreProperties = { "AdminComments", "TimeSentAt", "ReportJsonCriteria" };
        private string GetDirtyDesc(EntityEventData args)
        {
            var builder = new StringBuilder();
            var index = 1;
            foreach (var dirtyProperty in args.DirtyProperties)
            {
                if (IgnoreProperties.Any(x => x.Equals(args.PropertyNames[dirtyProperty], StringComparison.OrdinalIgnoreCase)))
                    continue;

                var row = GetDirtyrowDesc(args, index, dirtyProperty);
                if (string.IsNullOrWhiteSpace(row)) continue;
                builder.AppendLine(row);
                index++;
            }
            return builder.ToString();
        }
        private string GetDirtyrowDesc(EntityEventData args, int index, int dirtyProperty, string propertyName = null)
        {
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
            if (string.IsNullOrWhiteSpace(propertyName))
                propertyName = args.PropertyNames[dirtyProperty];
            var oldState = GetString(args.OldState[dirtyProperty]);

            if (args.State == null)
                return string.Empty;

            var newState = GetString(args.State[dirtyProperty]);
            if (string.IsNullOrWhiteSpace(oldState) && string.IsNullOrWhiteSpace(newState))
                return string.Empty;
            return string.Format(rowTemplate, index, propertyName, oldState, newState);
        }
        private string GetDirtyDesc(IList<EntityEventData> args)
        {
            var builder = new StringBuilder();
            foreach (var eventData in args)
            {
                builder.Append(GetDirtyDesc(eventData));
            }
            return builder.ToString();
        }
        private object GetState(EntityEventData args, string propertyName, bool isOld = false)
        {
            var propertyIndex = GetPropertyIndex(args, propertyName);
            return isOld ? args.OldState[propertyIndex] : args.State[propertyIndex];
        }
        private bool IsDirty(EntityEventData args, string propertyName)
        {
            var propertyIndex = GetPropertyIndex(args, propertyName);
            return args.DirtyProperties.Contains(propertyIndex);
        }
        private int GetPropertyIndex(EntityEventData args, string propertyName)
        {
            // get property index
            return Array.IndexOf(args.PropertyNames, propertyName);
            /*int propertyIndex;
            for (propertyIndex = 0; propertyIndex < args.PropertyNames.Length; propertyIndex++)
            {
                var property = args.PropertyNames[propertyIndex];
                if (property.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
            if (propertyIndex > args.PropertyNames.Length - 1)
            {
                throw new Exception("Property not found");
            }
            return propertyIndex;*/
        }
        private string GetString(object property)
        {
            if (property == null)
                return string.Empty;
            var entites = property as IEnumerable<IEntity<int>>;
            if (entites != null)
            {
                foreach (var obj in entites)
                {
                    _NhibernateSession.Lock(obj, LockMode.None);
                }
                var returnvalue = ((IEnumerable<IEntity<int>>)property).Aggregate(string.Empty, (current, item) => current + ("," + item.GetDescription()));
                return returnvalue.Trim(',');
            }
            var entity = property as IEntity<int>;
            if (entity != null)
            {
                try
                {
                    _NhibernateSession.Lock(entity, LockMode.None);
                    return entity.GetDescription();
                }
                catch (Exception ex)
                {
                    return GetName(entity);
                }
            }
            if (property is DateTime)
            {
                var dt = ((DateTime)property);
                return dt.ToShortDateString();//+ " " + dt.ToShortDateString();
            }
            if (property is decimal)
            {
                var value = (decimal)property;
                return FormatDecimal(value, "$");
            }
            return property.ToString();
        }
        protected string FormatDecimal(Decimal value, string extra = "")
        {
            if (value == 0)
            {
                return string.Format("0{0}", extra);
            }
            if (value < 1)
            {
                return string.Format("{0}{1}", value.ToString("0.##"), extra);
            }
            return string.Format("{0}{1}", value.ToString("#####################.##"), extra);
        }
        private string GetName(IEntity<int> entity)
        {
            if (entity is TileImage)
            {
                var item = tileImageRepository.Get(entity.ID);
                return item.GetDescription();
            }
            if (entity is ManagedLookupBase)
            {
                var lookupBase = entity as ManagedLookupBase;
                try
                {
                    return lookupBase.GetDescription();
                }
                catch (Exception)
                {
                    return GetName(lookupBase.ID);
                }
            }
            //try to get the name id
            /*try
            {
               
                  var nameProperty = entity.GetType().GetProperty("Name");
                  if (nameProperty != null)
                  {
                      var value = nameProperty.GetValue("Name", null);
                      NhibernateSession.Update(value);
                      return entity.GetDescription();
                  }
            }
            catch (Exception exception)
            {
                return string.Empty;
            }*/
            return string.Empty;
        }

        private string GetName(int id)
        {
            var name = localizedStringRepository.Get(id);
            return name.ToString();
        }
        private IList<AdCreative> GetActiveAds(IEnumerable<AdCreative> ads, bool isIncludeSubmitted = false)
        {
            var obj = ads.Select(adCreative => adCreativeRepository.Get(adCreative.ID)).ToList();
            if (!isIncludeSubmitted)
            {
                return (from x in obj
                        where x.Status.ID == AdCreativeStatus.Active.ID || x.Status.ID == AdCreativeStatus.ActiveAdServer.ID ||
                              x.Status.ID == AdCreativeStatus.BudgetPaused.ID ||
                              x.Status.ID == AdCreativeStatus.Completed.ID ||
                             (x.PausedStatus != null && (x.PausedStatus.ID == AdCreativeStatus.Active.ID || x.PausedStatus.ID == AdCreativeStatus.ActiveAdServer.ID)) ||
                              (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.BudgetPaused.ID) ||
                              (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.Completed.ID)
                        select x).ToList();
            }
            else
            {
                return (from x in obj
                        where x.Status.ID == AdCreativeStatus.Active.ID || x.Status.ID == AdCreativeStatus.ActiveAdServer.ID ||
                              x.Status.ID == AdCreativeStatus.BudgetPaused.ID ||
                              x.Status.ID == AdCreativeStatus.Submitted.ID ||
                              x.Status.ID == AdCreativeStatus.Completed.ID ||
                              (x.PausedStatus != null &&( x.PausedStatus.ID == AdCreativeStatus.Active.ID  ||  x.PausedStatus.ID == AdCreativeStatus.ActiveAdServer.ID  )) ||
                              (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.BudgetPaused.ID) ||
                              (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.Submitted.ID) ||
                              (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.Completed.ID)
                        select x).ToList();
            }

            /* return ads.SelectMany(x =>
                         x.Status.ID == AdCreativeStatus.Active.ID ||
                         x.Status.ID == AdCreativeStatus.BudgetPaused.ID ||
                         x.Status.ID == AdCreativeStatus.Completed.ID ||
                        (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.Active.ID) ||
                        (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.BudgetPaused.ID) ||
                        (x.PausedStatus != null && x.PausedStatus.ID == AdCreativeStatus.Completed.ID));*/
        }

        private List<EventEmails> GetEventsEmails()
        {
            ISQLQuery query =
                _NhibernateSession.CreateSQLQuery(
                    "call GetEmailSenderEventsEmails()");
            query.SetResultTransformer(Transformers.AliasToBean<EventEmails>());
            var items = query.List<EventEmails>().ToList() ?? new List<EventEmails>();

            return items;
        }

        private Dictionary<string, string> GetSendToEmails(string eventName)
        {
            EventEmails eventEmail = _EventsEmails.Where(p => string.Equals(p.EventName, eventName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            List<string> emailsList = null;

            string defaultEmail = config.GetConfigurationSetting(null, null, "EventBrokerEmail");

            if (eventEmail == null)
            {
                emailsList = new List<string>() { defaultEmail };
            }
            else
            {
                emailsList = eventEmail.EmailsList.ToList();

                if (eventEmail.SendToDefault)
                {
                    emailsList.Add(defaultEmail);
                }
            }

            return emailsList.ToDictionary(x => x, x => x);
        }

        #region Formatting
        #region Campaign
        private string GetAdCreativeChanges(IList<EntityEventData> eventDataList)
        {
            var eventDataAdActionValue = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdActionValue);
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            var builder = new StringBuilder();



            var index = 1;
            if (eventDataAdActionValue != null)
            {
                var valueEntity = eventDataAdActionValue.Entity as AdActionValue;
                var resourceKey = string.Format("AdActionValue_{0}", valueEntity.ActionType.ID < 6 ? 1 : valueEntity.ActionType.ID);
                var resourceKey2 = string.Format("AdActionValue2_{0}", valueEntity.ActionType.ID < 6 ? 1 : valueEntity.ActionType.ID);
                //check first property
                if (IsDirty(eventDataAdActionValue, "Value"))
                {
                    var proprtyname = ResourceManager.Instance.GetResource(resourceKey, "EventBroker_Emails");
                    builder.AppendLine(GetDirtyrowDesc(eventDataAdActionValue, index, GetPropertyIndex(eventDataAdActionValue, "Value"), proprtyname));
                    index++;
                }
                //check second property
                if (IsDirty(eventDataAdActionValue, "Value2"))
                {
                    var proprtyname = ResourceManager.Instance.GetResource(resourceKey2, "EventBroker_Emails");
                    builder.AppendLine(GetDirtyrowDesc(eventDataAdActionValue, index, GetPropertyIndex(eventDataAdActionValue, "Value2"), proprtyname));
                    index++;
                }

            }
            if (eventData != null)
            {
                foreach (var dirtyProperty in eventData.DirtyProperties)
                {
                    string propertyName = eventData.PropertyNames[dirtyProperty];
                    if (!string.IsNullOrEmpty(propertyName) && string.Equals(propertyName, "bid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var adCreative = (eventData.Entity as AdCreative);
                        string rowTemplate = @"<tr><td>{0}</td><td>Bid</td><td>{1}</td><td>{2}</td></tr>";
                        var oldState = eventData.OldState[dirtyProperty];
                        builder.AppendLine(string.Format(rowTemplate, index, GetString(Convert.ToDecimal(oldState) * adCreative.Group.CostModelWrapper.Factor), GetString(adCreative.GetReadableBid())));
                    }
                    else
                    {
                        builder.AppendLine(GetDirtyrowDesc(eventData, index, dirtyProperty));
                    }

                    index++;
                }
            }
            return builder.ToString();
        }
        private string GetAdDesc(AdCreative adCreative)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";

            //App Site Description

            //Ad Name
            builder.Append(string.Format(rowTemplate, index, "Name", adCreative.Name));
            index++;

            //Ad Text
            builder.Append(string.Format(rowTemplate, index, "Ad Text", adCreative.AdText));
            index++;

            //Ad Type
            builder.Append(string.Format(rowTemplate, index, "Ad Type", adCreative.TypeId.ToString()));
            index++;

            //Device Type
            builder.Append(string.Format(rowTemplate, index, "Device Type", adCreative.CretiveUnitDeviceType.ToString()));
            index++;
            string CreativeVendor = GetCreativeVendorDes(adCreative.AdCreativeUnits);
            if (!string.IsNullOrEmpty(CreativeVendor))
            {
                //Creative Vendor
                builder.Append(string.Format(rowTemplate, index, "Creative Vendor", CreativeVendor));
                index++;
            }


            //Enviroment Type
            builder.Append(string.Format(rowTemplate, index, "Enviroment Type", adCreative.EnvironmentType!=null ? adCreative.EnvironmentType.ToString(): EnvironmentType.All.ToString()));
            index++;
            //IsSecureComplimant Type
            builder.Append(string.Format(rowTemplate, index, "Is Secure Compliant", adCreative.IsSecureCompliant.ToString()));
            index++;
            if (adCreative.TypeId != AdTypeIds.NativeAd)
            {
                //Orientation Type
                builder.Append(string.Format(rowTemplate, index, "Orientation Type", adCreative.OrientationType!=null?  adCreative.OrientationType.ToString() : OrientationType.Any.ToString()));
                index++;
            }

            //RichMedia
            if (adCreative.TypeId == AdTypeIds.RichMedia)
            {
                RichMediaCreative richMedia = (adCreative as RichMediaCreative);
                //Ad Text alid
                builder.Append(string.Format(rowTemplate, index, "Rich Media Required Protocol", (richMedia.GetRichMediaProtocol() == null ? "None" : richMedia.GetRichMediaProtocol().ToString())));
                index++;
            }

            if (adCreative.TypeId == AdTypeIds.NativeAd)
            {
                if (IsDownloadAction(adCreative.Group.Objective.AdAction.ID))
                {
                    NativeAdCreative nativeAd = (adCreative as NativeAdCreative);

                    builder.Append(string.Format(rowTemplate, index, "Description", nativeAd.Description == null ? "None" : nativeAd.Description));
                    index++;

                    builder.Append(string.Format(rowTemplate, index, "Show if App Installed", nativeAd.ShowIfInstalled));
                    index++;

                    builder.Append(string.Format(rowTemplate, index, "Star Rating", (!nativeAd.StarRating.HasValue ? "None" : nativeAd.StarRating.Value.ToString())));
                    index++;

                    builder.Append(string.Format(rowTemplate, index, "Action Text", (nativeAd.ActionText == null ? "None" : nativeAd.ActionText)));
                    index++;

                    builder.Append(string.Format(rowTemplate, index, "App Open Url", (nativeAd.AppOpenUrl == null ? "None" : nativeAd.AppOpenUrl)));
                    index++;
                }
            }

            if (adCreative.ActionValue != null)
            {
                //TODO:OSaleh to change the label depend on the ad action

                if (!string.IsNullOrEmpty(adCreative.ActionValue.Value))
                {
                    var resourceKey = string.Format("AdActionValue_{0}", adCreative.ActionValue.ActionType.ID < 6 ? 1 : adCreative.ActionValue.ActionType.ID);
                    var proprtyname = ResourceManager.Instance.GetResource(resourceKey, "EventBroker_Emails");
                    builder.Append(string.Format(rowTemplate, index, proprtyname, adCreative.ActionValue.Value));
                    index++;
                }

                if (!string.IsNullOrWhiteSpace(adCreative.ActionValue.Value2))
                {
                    var resourceKey2 = string.Format("AdActionValue2_{0}", adCreative.ActionValue.ActionType.ID < 6 ? 1 : adCreative.ActionValue.ActionType.ID);

                    var proprtyname = ResourceManager.Instance.GetResource(resourceKey2, "EventBroker_Emails");
                    builder.Append(string.Format(rowTemplate, index, proprtyname, adCreative.ActionValue.Value2));
                    index++;
                }
            }
            if (adCreative is AdTrackerCreative)
            {
                var item = adCreative as AdTrackerCreative;
                //Tile Image
                var resourceKey = "AppMarketingPartnerName";
                var proprtyname = ResourceManager.Instance.GetResource(resourceKey, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname, item.AppMarketingPartner.Description));
                index++;
                var resourceKey2 = "ClickTrackerUrl";
                var proprtyname2 = ResourceManager.Instance.GetResource(resourceKey2, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname2, item.ClickTrackerUrl));
                index++;

                var resourceKey7 = "EnableEventsPostback";
                var proprtyname7 = ResourceManager.Instance.GetResource(resourceKey7, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname7, item.EnableEventsPostback.ToString()));
                index++;
                var resourceKey8 = "VerifyTargetingCriteria";
                var proprtyname8 = ResourceManager.Instance.GetResource(resourceKey8, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname8, item.VerifyTargetingCriteria.ToString()));
                index++;

                var resourceKey9 = "UpdateEventsFrequency";
                var proprtyname9 = ResourceManager.Instance.GetResource(resourceKey9, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname9, item.UpdateEventsFrequency.ToString()));
                index++;



                var resourceKey3 = "VerifyDailyBudget";
                var proprtyname3 = ResourceManager.Instance.GetResource(resourceKey3, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname3, item.VerifyDailyBudget.ToString()));
                index++;

                var resourceKey4 = "VerifyCampaignStartAndEndDate";
                var proprtyname4 = ResourceManager.Instance.GetResource(resourceKey4, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname4, item.VerifyCampaignStartAndEndDate.ToString()));
                index++;

                var resourceKey5 = "UpdateTags";
                var proprtyname5 = ResourceManager.Instance.GetResource(resourceKey5, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname5, item.UpdateTags.ToString()));
                index++;
                var resourceKey6 = "VerifyEventsFrequency";
                var proprtyname6 = ResourceManager.Instance.GetResource(resourceKey6, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname6, item.VerifyEventsFrequency.ToString()));
                index++;

                var resourceKey10 = "VerifyPrerequisiteEvents";
                var proprtyname10= ResourceManager.Instance.GetResource(resourceKey10, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname10, item.VerifyEventsFrequency.ToString()));
                index++;

                var resourceKey11 = "ValidateRequestDeviceAndLocationData";
                var proprtyname11 = ResourceManager.Instance.GetResource(resourceKey11, "TrackingAd");
                builder.Append(string.Format(rowTemplate, index, proprtyname11, item.ValidateRequestDeviceAndLocationData.ToString()));
                index++;


                


            }

            if (adCreative is TextCreative)
            {
                var item = adCreative as TextCreative;
                //Tile Image
                var tileImageName = "Custom";
                if (!item.TileImage.IsCustom)
                {
                    tileImageName = item.TileImage.GetDescription();
                }
                builder.Append(string.Format(rowTemplate, index, "Tile Image", tileImageName));
                index++;
            }

            //Ad Type
            builder.Append(string.Format(rowTemplate, index, "Bid", GetString(adCreative.GetReadableBid())));
            index++;

            return builder.ToString();
        }
        private string GetCreativeVendorDes(IList<AdCreativeUnit> AdCreativeUnits)
        {
            var AdCreativeUnit = AdCreativeUnits.FirstOrDefault();
            string AdCreativeUnitVendors = "";
            if (AdCreativeUnit != null)
            {
                foreach (AdCreativeUnitVendor item in AdCreativeUnit.AdCreativeUnitVendorList)
                {
                    AdCreativeUnitVendors += item.Vendor.Name.Value + ", ";
                }
            }
            return AdCreativeUnitVendors;
        }
        private string GetGroupDesc(AdGroup groupObj)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string template = @"<p>AdGroup:<b> {0}</b></p>{1}<br />";
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            const string DescTemplate = @"<table> <tr> <td>ID</td> <td>Name</td><td>Value</td></tr>{0}</table>";
            //AdGroup Description
            index = 1;
            builder = new StringBuilder();

            //Objective
            builder.Append(string.Format(rowTemplate, index, "Objective", groupObj.Objective.GetDescription()));
            index++;

            //Bid
            builder.Append(string.Format(rowTemplate, index, "Bid", GetString(groupObj.GetReadableBid())));
            index++;

            //MinimumUnitPrice
            builder.Append(string.Format(rowTemplate, index, "MinimumUnitPrice", GetString(groupObj.MinimumUnitPrice)));
            index++;
            //Cost Model
            builder.Append(string.Format(rowTemplate, index, "Cost Model", groupObj.CostModelWrapperEnum.ToString()));
            index++;

            if (groupObj.DailyBudget.HasValue)
            {

                //Daily Budget
                builder.Append(string.Format(rowTemplate, index, "Daily Budget", GetString(groupObj.DailyBudget.Value)));
                index++;
            }

            if (groupObj.Budget.HasValue)
            {

                // Budget
                builder.Append(string.Format(rowTemplate, index, "Budget", GetString(groupObj.Budget.Value)));
                index++;
            }
            //Track installs
            builder.Append(string.Format(rowTemplate, index, "Track Installs", groupObj.TrackInstalls.ToString()));
            index++;
            //Open In External Broswer
            builder.Append(string.Format(rowTemplate, index, "Open In External Browser", groupObj.OpenInExternalBrowser.ToString()));
            index++;


            //Targeting
            builder.Append(string.Format(rowTemplate, index, "Targeting", GetTargetingDesc(groupObj)));
            index++;

            var bidConfigChanges = GetBidConfigDesc(groupObj);
            //bidConfig
            builder.Append(string.Format(rowTemplate, index, "Bid Config", bidConfigChanges));
            index++;

            var CostElementChanges = GetCostElementDesc(groupObj);
            //costElements
            builder.Append(string.Format(rowTemplate, index, "Cost Element", CostElementChanges));
            index++;


            var groupDesc = string.Format(DescTemplate, builder.ToString());

            return string.Format(template, groupObj.Name, groupDesc);
        }
        private string GetCampaignDesc(Campaign campaignObj)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string template = @"<p>Campaign:<b> {0}</b></p>{1}<br/>";
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            const string DescTemplate = @"<table> <tr> <td>ID</td> <td>Name</td><td>Value</td></tr>{0}</table>";
            string campaignDesc = string.Empty;

            //campaign Description

            //Start Date
            builder.Append(string.Format(rowTemplate, index, "Start Date", GetString(campaignObj.StartDate)));
            index++;

            //End Date
            var endDate = "Open";
            if (campaignObj.EndDate.HasValue)
            {
                endDate = GetString(campaignObj.EndDate.Value);
            }
            builder.Append(string.Format(rowTemplate, index, "End Date", endDate));
            index++;

            //Budget
            builder.Append(string.Format(rowTemplate, index, "Budget", GetString(campaignObj.Budget)));
            index++;

            //DailyBudget
            var dailyBudget = "None";
            if (campaignObj.DailyBudget.HasValue)
            {
                dailyBudget = GetString(campaignObj.DailyBudget.Value);
            }
            builder.Append(string.Format(rowTemplate, index, "Daily Budget", dailyBudget));
            index++;

            // Type
            builder.Append(string.Format(rowTemplate, index, "Campaign Type", Enum.GetName(typeof(CampaignType), campaignObj.CampaignType)));
            index++;

            campaignDesc = string.Format(DescTemplate, builder.ToString());

            return string.Format(template, campaignObj.Name, campaignDesc);
        }

        private string GetCampaignGroupDesc(Campaign campaignObj, AdGroup groupObj)
        {

            return string.Format("{0}{1}", GetCampaignDesc(campaignObj), GetGroupDesc(groupObj));
        }
        private TargetingBase getTargetingBase(TargetingBase targetingBase, AdGroup adGroupObj)
        {
            return adGroupObj.Targetings.FirstOrDefault(item => item.ID == targetingBase.ID);
        }
        private string GetTargetingDesc(AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var index = 1;
            var value = string.Empty;
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            //const string template =@"<table> <tr> <td>ID</td> <td>Name</td> <td>Value</td></tr>{0}</table>";
            const string template = @"<table>{0}</table>";

            //Keywords
            var targetingEventDataKeyword = adGroupObj.Targetings.ToList().OfType<KeywordTargeting>().ToList();
            value = targetingEventDataKeyword.Aggregate(value, (current, data) => current + ("," + data.GetDescription()));
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim(',');
                builder.Append(string.Format(rowTemplate, index, "Keyword", value));
                index++;
            }

            //PMPDeal
            var targetingEventDataPMPDeal = adGroupObj.Targetings.ToList().OfType<AdPMPDealTargeting>().ToList();
            value = targetingEventDataPMPDeal.Aggregate(value, (current, data) => current + ("," + data.Deal.Name));
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim(',');
                builder.Append(string.Format(rowTemplate, index, "PMPDeal", value));
                index++;
            }

            //Audiance
            var targetingEventDataAudiance = adGroupObj.Targetings.ToList().OfType<AudienceSegmentTargeting>().ToList();
            if (targetingEventDataAudiance != null)
            {
                value = targetingEventDataAudiance.Aggregate(value, (current, data) => current + ("," + (data!=null &&  data.AudienceSegment!=null &&  data.AudienceSegment.Name!=null && data.AudienceSegment.Name.Value!=null? data.AudienceSegment.Name.ToString():string.Empty)));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.Trim(',');
                    builder.Append(string.Format(rowTemplate, index, "Audiance", value));
                    index++;
                }
            }

            //Geographic
            value = string.Empty;
            var targetingEventDataGeographic = adGroupObj.Targetings.ToList().OfType<GeographicTargeting>().ToList();
            value = targetingEventDataGeographic.Aggregate(value, (current, data) => current + ("," + data.GetDescription()));
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim(',');
                builder.Append(string.Format(rowTemplate, index, "Geographic", value));
                index++;
            }

            //Operator
            value = string.Empty;
            var targetingEventDataOperator = adGroupObj.Targetings.ToList().OfType<OperatorTargeting>().ToList();
            value = targetingEventDataOperator.Aggregate(value, (current, data) => current + ("," + data.GetDescription()));
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim(',');
                builder.Append(string.Format(rowTemplate, index, "Operator", value));
                index++;
            }

            //device
            var deviceTargeting = adGroupObj.Targetings.ToList().OfType<DeviceTargeting>().FirstOrDefault();
            if (deviceTargeting != null)
            {
                var deviceResult = deviceTargeting.GetDescription();
                if (!string.IsNullOrWhiteSpace(deviceResult))
                {
                    deviceResult = deviceResult.Trim(',');
                    builder.Append(string.Format(rowTemplate, index, "Device Targeting", deviceResult));
                    index++;
                }
            }


            var fields = builder.ToString();
            return string.IsNullOrWhiteSpace(fields) ? string.Empty : string.Format(template, fields);
        }
        private string GetDeviceTargetingDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj, int index)
        {

            var devicetargting = adGroupObj.Targetings.ToList().OfType<DeviceTargeting>().FirstOrDefault();
            if (devicetargting != null)
            {
                const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
                var builder = new StringBuilder();
                var deletedText = string.Empty;
                var InsertText = string.Empty;

                //Platform
                var targetingEventData_platform_Delete = (from data in targetingEventData
                                                          where
                                                              data.Entity is PlatformTargeting &&
                                                              data.ActionType == ObjectActionEnum.Delete
                                                          select data).ToList();
                var targetingEventData_platform_Insert = (from data in targetingEventData
                                                          where
                                                              data.Entity is PlatformTargeting &&
                                                              data.ActionType == ObjectActionEnum.Insert
                                                          select data).ToList();
                var targetingEventData_platform_Update = (from data in targetingEventData
                                                          where
                                                              data.Entity is PlatformTargeting &&
                                                              data.ActionType == ObjectActionEnum.Update
                                                          select data).ToList();


                foreach (var data in targetingEventData_platform_Delete)
                {
                    if ((data.Entity as PlatformTargeting).IsAll)
                    {
                        var name_entity = platformRepository.Get((data.Entity as PlatformTargeting).Platform.ID);
                        deletedText += ("," + name_entity.GetDescription());
                    }
                }
                foreach (EntityEventData data in targetingEventData_platform_Insert)
                {
                    //if ((devicetargting.TargetingType.ID== DeviceTargetingType.ModelTargetingTypeId) || (data.Entity as PlatformTargeting).IsAll)
                    if ((data.Entity as PlatformTargeting).IsAll)
                    {
                        data.Entity = devicetargting.GetTargeting((data.Entity as IEntity<int>).ID, DeviceTargetingTypeEnum.Platform);
                        //getTargetingBase(data.Entity as PlatformTargeting, adGroupObj);
                        InsertText += ("," + (data.Entity as PlatformTargeting).GetDescription());
                    }
                }
                foreach (EntityEventData data in targetingEventData_platform_Update)
                {
                    if ((devicetargting.TargetingType.ID == DeviceTargetingType.ModelTargetingTypeId) || (data.Entity as PlatformTargeting).IsAll)
                    {
                        data.Entity = devicetargting.GetTargeting((data.Entity as IEntity<int>).ID, DeviceTargetingTypeEnum.Platform);
                        InsertText += ("," + (data.Entity as PlatformTargeting).GetDescription());
                    }
                    else
                    {
                        data.Entity = devicetargting.GetTargeting((data.Entity as IEntity<int>).ID, DeviceTargetingTypeEnum.Platform);
                        deletedText += ("," + (data.Entity as PlatformTargeting).GetDescription());
                    }

                }
                deletedText = deletedText.Trim(',');
                InsertText = InsertText.Trim(',');
                if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
                {
                    builder.Append(string.Format(rowTemplate, index, "Platform", deletedText, InsertText));
                    deletedText = InsertText = string.Empty;
                    index++;
                }


                //Manufacturer
                var paltformTargeting = devicetargting.PlatformsTargeting.FirstOrDefault();
                var targetingEventData_manufacturer_Delete = (from data in targetingEventData
                                                              where
                                                                  data.Entity is ManufacturerTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Delete
                                                              select data).ToList();
                var targetingEventData_manufacturer_Insert = (from data in targetingEventData
                                                              where
                                                                  data.Entity is ManufacturerTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Insert
                                                              select data).ToList();
                var targetingEventData_manufacturer_Update = (from data in targetingEventData
                                                              where
                                                                  data.Entity is ManufacturerTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Update
                                                              select data).ToList();

                foreach (var data in targetingEventData_manufacturer_Delete)
                {
                    if ((devicetargting.TargetingType.ID == DeviceTargetingType.ModelTargetingTypeId) || (data.Entity as ManufacturerTargeting).IsAll)
                    {
                        var name_entity = manufacturerRepository.Get((data.Entity as ManufacturerTargeting).Manufacturer.ID);
                        deletedText += ("," + name_entity.GetDescription());
                    }
                }
                foreach (EntityEventData data in targetingEventData_manufacturer_Insert)
                {
                    //if ((devicetargting.TargetingType.ID == DeviceTargetingType.ModelTargetingTypeId) || (data.Entity as ManufacturerTargeting).IsAll)
                    if ((data.Entity as ManufacturerTargeting).IsAll)
                    {
                        var name_entity = manufacturerRepository.Get((data.Entity as ManufacturerTargeting).Manufacturer.ID);
                        InsertText += ("," + name_entity.GetDescription());
                    }
                }
                foreach (EntityEventData data in targetingEventData_manufacturer_Update)
                {
                    if ((data.Entity as ManufacturerTargeting).IsAll)
                    {
                        var name_entity = manufacturerRepository.Get((data.Entity as ManufacturerTargeting).Manufacturer.ID);
                        InsertText += ("," + name_entity.GetDescription());
                    }
                    else
                    {
                        var name_entity = manufacturerRepository.Get((data.Entity as ManufacturerTargeting).Manufacturer.ID);
                        deletedText += ("," + name_entity.GetDescription());
                    }
                }

                deletedText = deletedText.Trim(',');
                InsertText = InsertText.Trim(',');
                if ((paltformTargeting != null) && (devicetargting.TargetingType.ID == DeviceTargetingType.ActionTypeTargetingTypeId) && (!paltformTargeting.IsAll))
                {
                    if (!string.IsNullOrWhiteSpace(deletedText))
                    {
                        deletedText = string.Format("{0}({1})", paltformTargeting.Platform.GetDescription(), deletedText);
                    }
                    if (!string.IsNullOrWhiteSpace(InsertText))
                    {
                        InsertText = string.Format("{0}({1})", paltformTargeting.Platform.GetDescription(), InsertText);
                    }
                }
                if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
                {
                    builder.Append(string.Format(rowTemplate, index, "Manufacturer", deletedText, InsertText));
                    deletedText = InsertText = string.Empty;
                    index++;
                }

                //Model
                var targetingEventData_model_Delete = (from data in targetingEventData
                                                       where
                                                           data.Entity is ModelTargeting &&
                                                           data.ActionType == ObjectActionEnum.Delete
                                                       select data).ToList();
                var targetingEventData_model_Insert = (from data in targetingEventData
                                                       where
                                                           data.Entity is ModelTargeting &&
                                                           data.ActionType == ObjectActionEnum.Insert
                                                       select data).ToList();
                foreach (var data in targetingEventData_model_Delete)
                {
                    var name_entity = deviceRepository.Get((data.Entity as ModelTargeting).Device.ID);
                    deletedText += ("," + name_entity.GetDescription());
                }
                foreach (EntityEventData data in targetingEventData_model_Insert)
                {
                    data.Entity = devicetargting.GetTargeting((data.Entity as IEntity<int>).ID, DeviceTargetingTypeEnum.ModelTargeting);
                    InsertText += ("," + (data.Entity as ModelTargeting).GetDescription());
                }

                deletedText = deletedText.Trim(',');
                InsertText = InsertText.Trim(',');
                if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
                {
                    builder.Append(string.Format(rowTemplate, index, "Model", deletedText, InsertText));
                    deletedText = InsertText = string.Empty;
                    index++;
                }
                //Device Capability
                var targetingEventData_deviceCapability_Delete = (from data in targetingEventData
                                                                  where
                                                                      data.Entity is DeviceCapabilityTargeting &&
                                                                      data.ActionType == ObjectActionEnum.Delete
                                                                  select data).ToList();
                var targetingEventData_deviceCapability_Insert = (from data in targetingEventData
                                                                  where
                                                                      data.Entity is DeviceCapabilityTargeting &&
                                                                      data.ActionType == ObjectActionEnum.Insert
                                                                  select data).ToList();

                foreach (var data in targetingEventData_deviceCapability_Delete)
                {
                    var name_entity = deviceCapabilityRepository.Get((data.Entity as DeviceCapabilityTargeting).Capability.ID);
                    deletedText += ("," + name_entity.GetDescription());
                }
                foreach (EntityEventData data in targetingEventData_deviceCapability_Insert)
                {
                    data.Entity = devicetargting.GetTargeting((data.Entity as IEntity<int>).ID, DeviceTargetingTypeEnum.DeviceCapability);
                    InsertText += ("," + (data.Entity as DeviceCapabilityTargeting).GetDescription());
                }

                deletedText = deletedText.Trim(',');
                InsertText = InsertText.Trim(',');
                if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
                {
                    builder.Append(string.Format(rowTemplate, index, "Capability", deletedText, InsertText));
                    deletedText = InsertText = string.Empty;
                    index++;
                }
                return builder.ToString();
            }
            return string.Empty;
        }
        private string GetTargetingDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var index = 1;
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
            const string template = @"<br />Targeting changes<br /><table> <tr> <td>ID</td> <td>Name</td> <td>Deleted</td> <td>Added</td> </tr>@Fields</table>";

            //Keywords
            var targetingEventData_keyword_Delete = (from data in targetingEventData
                                                     where
                                                         data.Entity is KeywordTargeting &&
                                                         data.ActionType == ObjectActionEnum.Delete
                                                     select data).ToList();
            var targetingEventData_keyword_Insert = (from data in targetingEventData
                                                     where
                                                         data.Entity is KeywordTargeting &&
                                                         data.ActionType == ObjectActionEnum.Insert
                                                     select data).ToList();

            foreach (var data in targetingEventData_keyword_Delete)
            {
                var name_entity = keyWordRepository.Get((data.Entity as KeywordTargeting).Keyword.ID);
                deletedText += ("," + name_entity.GetDescription());
            }
            foreach (EntityEventData data in targetingEventData_keyword_Insert)
            {
                data.Entity = getTargetingBase(data.Entity as KeywordTargeting, adGroupObj);
                InsertText += ("," + (data.Entity as IEntity<int>).GetDescription());
            }

            deletedText = deletedText.Trim(',');
            InsertText = InsertText.Trim(',');
            if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
            {
                builder.Append(string.Format(rowTemplate, index, "Keyword", deletedText, InsertText));
                index++;
            }



            //Keywords
            var targetingEventData_Language_Delete = (from data in targetingEventData
                                                      where
                                                          data.Entity is LanguageTargeting &&
                                                          data.ActionType == ObjectActionEnum.Delete
                                                      select data).ToList();
            var targetingEventData_Language_Insert = (from data in targetingEventData
                                                      where
                                                          data.Entity is LanguageTargeting &&
                                                          data.ActionType == ObjectActionEnum.Insert
                                                      select data).ToList();

            foreach (var data in targetingEventData_Language_Delete)
            {
                var name_entity = LanguageRepository.Get((data.Entity as LanguageTargeting).Language.ID);
                deletedText += ("," + name_entity.GetDescription());
            }
            foreach (EntityEventData data in targetingEventData_Language_Insert)
            {
                data.Entity = getTargetingBase(data.Entity as LanguageTargeting, adGroupObj);
                InsertText += ("," + (data.Entity as IEntity<int>).GetDescription());
            }

            deletedText = deletedText.Trim(',');
            InsertText = InsertText.Trim(',');
            if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
            {
                builder.Append(string.Format(rowTemplate, index, "Language", deletedText, InsertText));
                index++;
            }

            //PMPDEAL
            var PMPDEALDelete = (from data in targetingEventData
                                 where
                                     data.Entity is AdPMPDealTargeting &&
                                     data.ActionType == ObjectActionEnum.Delete
                                 select data).ToList();
            var PMPDEALInsert = (from data in targetingEventData
                                 where
                                     data.Entity is AdPMPDealTargeting &&
                                     data.ActionType == ObjectActionEnum.Insert
                                 select data).ToList();

            foreach (var data in PMPDEALDelete)
            {
                var entity = (data.Entity as AdPMPDealTargeting) != null ? (data.Entity as AdPMPDealTargeting).Deal : null;
                if (entity != null)
                {
                    deletedText += ("," + entity.Name);
                }
            }
            foreach (EntityEventData data in PMPDEALInsert)
            {
                var entity = (data.Entity as AdPMPDealTargeting) != null ? (data.Entity as AdPMPDealTargeting).Deal : null;
                if (entity != null)
                {

                    InsertText += ("," + entity.Name);
                }
            }

            deletedText = deletedText.Trim(',');
            InsertText = InsertText.Trim(',');
            if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
            {
                builder.Append(string.Format(rowTemplate, index, "PMPDeaL", deletedText, InsertText));
                index++;
            }





            //Geographic
            deletedText = string.Empty;
            InsertText = string.Empty;
            var targetingEventDataGeographicDelete = (from data in targetingEventData
                                                      where
                                                          data.Entity is GeographicTargeting &&
                                                          data.ActionType == ObjectActionEnum.Delete
                                                      select data).ToList();
            var targetingEventDataGeographicInsert = (from data in targetingEventData
                                                      where
                                                          data.Entity is GeographicTargeting &&
                                                          data.ActionType == ObjectActionEnum.Insert
                                                      select data).ToList();

            foreach (var data in targetingEventDataGeographicDelete)
            {
                var name_entity = locationRepository.Get((data.Entity as GeographicTargeting).Location.ID);
                deletedText += ("," + name_entity.GetDescription());
            }
            foreach (var data in targetingEventDataGeographicInsert)
            {
                data.Entity = getTargetingBase(data.Entity as GeographicTargeting, adGroupObj);
                InsertText += ("," + (data.Entity as IEntity<int>).GetDescription());
            }

            deletedText = deletedText.Trim(',');
            InsertText = InsertText.Trim(',');
            if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
            {
                builder.Append(string.Format(rowTemplate, index, "Geographic", deletedText, InsertText));
                index++;
            }


            //Operator
            deletedText = string.Empty;
            InsertText = string.Empty;
            var targetingEventData_Operator_Delete = (from data in targetingEventData
                                                      where
                                                          data.Entity is OperatorTargeting &&
                                                          data.ActionType == ObjectActionEnum.Delete
                                                      select data).ToList();
            var targetingEventData_Operator_Insert = (from data in targetingEventData
                                                      where
                                                          data.Entity is OperatorTargeting &&
                                                          data.ActionType == ObjectActionEnum.Insert
                                                      select data).ToList();

            foreach (var data in targetingEventData_Operator_Delete)
            {
                var name_entity = operatorRepository.Get((data.Entity as OperatorTargeting).Operator.ID);
                deletedText += ("," + name_entity.GetDescription());
            }
            foreach (var data in targetingEventData_Operator_Insert)
            {
                data.Entity = getTargetingBase(data.Entity as OperatorTargeting, adGroupObj);
                InsertText += ("," + (data.Entity as IEntity<int>).GetDescription());
            }

            deletedText = deletedText.Trim(',');
            InsertText = InsertText.Trim(',');
            if (!string.IsNullOrWhiteSpace(deletedText) || !string.IsNullOrWhiteSpace(InsertText))
            {
                builder.Append(string.Format(rowTemplate, index, "Operator", deletedText, InsertText));
                index++;
            }

            //Demographic
            var demographicetargting = targetingEventData.FirstOrDefault(data => data.Entity is DemographicTargeting);
            if (demographicetargting != null)
            {
                var demographic_entity = (demographicetargting.Entity as DemographicTargeting);
                var desc = string.Format("Gender:{0},AgeGroup:{1}", demographic_entity.Demographic.Gender == null ? "Both" : genderRepository.Get(demographic_entity.Demographic.Gender.ID).Name.ToString()
                                                                  , demographic_entity.Demographic.AgeGroup == null ? "All" : ageGroupRepository.Get(demographic_entity.Demographic.AgeGroup.ID).Name.ToString());
                deletedText = InsertText = string.Empty;
                switch (demographicetargting.ActionType)
                {
                    case ObjectActionEnum.Delete:
                        {
                            deletedText = desc; //demographicetargting.Entity.GetDescription();
                            break;
                        }
                    case ObjectActionEnum.Insert:
                        {
                            InsertText = desc; //demographicetargting.Entity.GetDescription();
                            break;
                        }
                    case ObjectActionEnum.Update:
                        {

                            var old_Demographic = GetState(demographicetargting, "Demographic", isOld: true) as Demographic;
                            deletedText = string.Format("Gender:{0},AgeGroup:{1}", old_Demographic.Gender == null ? "Both"
                                                            : genderRepository.Get(old_Demographic.Gender.ID).Name.
                                                                  ToString()
                                                        , old_Demographic.AgeGroup == null
                                                            ? "All" : ageGroupRepository.Get(old_Demographic.AgeGroup.ID).Name.ToString());
                            InsertText = desc; //demographicetargting.Entity.GetDescription();
                            break;
                        }

                }
                builder.Append(string.Format(rowTemplate, index, "Demographic", deletedText, InsertText));
                index++;
            }

            //device
            var deviceResult = GetDeviceTargetingDesc(targetingEventData, adGroupObj, index);
            if (!string.IsNullOrWhiteSpace(deviceResult))
            {
                builder.Append(deviceResult);
            }



            var fields = builder.ToString();
            return string.IsNullOrWhiteSpace(fields) ? string.Empty : template.Replace("@Fields", fields);
        }
        private string GetInventorySourceChangesDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />AdGroup Inventory Source Added<br /><table> <tr> <td>ID</td> <td>Partner</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Include</td> </tr>@Fields</table>";
            const string templateUpdated = @"<br />AdGroup Inventory Source Updated<br /><table> <tr> <td>ID</td><td>Partner</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Include</td> </tr>@Fields</table>";
            const string templateDeleted = @"<br />AdGroup Inventory Source Deleted<br /><table> <tr> <td>ID</td><td>Partner</td>  <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Include</td> </tr>@Fields</table>";

            string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";


            var targetingEventData_BidConfigs_Insert = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupInventorySource &&
                                                            data.ActionType == ObjectActionEnum.Insert
                                                        select data).ToList();

            var targetingEventData_BidConfigs_Update = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupInventorySource &&
                                                            data.ActionType == ObjectActionEnum.Update
                                                        select data).ToList();

            var targetingEventData_BidConfigs_Delete = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupInventorySource &&
                                                            data.ActionType == ObjectActionEnum.Delete
                                                        select data).ToList();

            foreach (var data in targetingEventData_BidConfigs_Insert)
            {
                var campaignBidConfig = (data.Entity as AdGroupInventorySource);

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.Partner.Name, campaignBidConfig.AppSite != null ? campaignBidConfig.AppSite.Name : string.Empty, campaignBidConfig.SubAppsite != null ? campaignBidConfig.SubPublisherId : string.Empty, campaignBidConfig.Include));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_BidConfigs_Update)
            {
                var campaignBidConfigObj = (data.Entity as AdGroupInventorySource);

                var campaignBidConfig = adGroupObj.GetAdGroupInventorySources().Where(x => x.ID == campaignBidConfigObj.ID).FirstOrDefault();

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.Partner.Name, campaignBidConfig.AppSite != null ? campaignBidConfig.AppSite.Name : string.Empty, campaignBidConfig.SubAppsite != null ? campaignBidConfig.SubPublisherId : string.Empty, campaignBidConfig.Include));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateUpdated.Replace("@Fields", fields));
            }
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_BidConfigs_Delete)
            {
                //data.Entity = adGroupObj.CampaignBidConfigs.Where(x => x.ID == (data.Entity as CampaignBidConfig).ID);
                //    deletedText += ("," + (data.Entity as CampaignBidConfig).GetDescription());
                var campaignBidConfigObj = (data.Entity as AdGroupInventorySource);

                var campaignBidConfig = adGroupObj.GetAdGroupInventorySources(true).Where(x => x.ID == campaignBidConfigObj.ID).FirstOrDefault();

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.Partner.Name, campaignBidConfig.AppSite != null ? campaignBidConfig.AppSite.Name : string.Empty, campaignBidConfig.SubAppsite != null ? campaignBidConfig.SubPublisherId : string.Empty, campaignBidConfig.Include));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateDeleted.Replace("@Fields", fields));
            }

            return template.ToString();
        }

        private string GetImpressionMetricChangesDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />Impression Metric Added<br />  <table> <tr> <td>Type</td>  <td>Value</td> <td>Ignore</td></tr>@Fields</table>";
            //const string templateUpdated = @"<br />Audiance Updated<br /><table> <tr> <td>Query</td> </tr>@Fields</table>";
            const string templateDeleted = @"<br />Impression Metric Deleted<br /><table> <tr> <td>Type</td>  <td>Value</td> <td>Ignore</td> </tr>@Fields</table>";

            string rowTemplate = @"<tr><td>{0}</td> <td>{0}</td> <td>{0}</td></tr>";


            var targetingEventData_ImpressionMetric_Insert = (from data in targetingEventData
                                                              where
                                                                  data.Entity is ImpressionMetricTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Insert
                                                              select data).ToList();

            //var targetingEventData_Audiance_Update = (from data in targetingEventData
            //                                          where
            //                                              data.Entity is ImpressionMetricTargeting &&
            //                                              data.ActionType == ObjectActionEnum.Update
            //                                          select data).ToList();

            var targetingEventData_ImpressionMetric_Delete = (from data in targetingEventData
                                                              where
                                                                  data.Entity is ImpressionMetricTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Delete
                                                              select data).ToList();

            foreach (var data in targetingEventData_ImpressionMetric_Insert)
            {
                var minvalue = (data.Entity as ImpressionMetricTargeting).MinValue;
                var ImpressionMetricType = (data.Entity as ImpressionMetricTargeting).ImpressionMetric.GetDescription();
                bool Ignore = (data.Entity as ImpressionMetricTargeting).Ignore;
                builder.AppendLine(string.Format(rowTemplate, ImpressionMetricType, minvalue, Ignore.ToString()));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();


            foreach (EntityEventData data in targetingEventData_ImpressionMetric_Delete)
            {

                builder.AppendLine(string.Format(rowTemplate, "()"));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateDeleted.Replace("@Fields", fields));
            }

            return template.ToString();

        }

        private string GetLanguageTargetingChangesDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />Language Targeting Added<br />  <table> <tr> <td>Value</td> </tr>@Fields</table>";
            const string templateDeleted = @"<br />Language Targeting Deleted<br/><table><tr><td>Value</td>  </tr>@Fields</table>";

            string rowTemplate = @"<tr><td>{0}</td></tr>";


            var targetingEventData_LanguageTargeting_Insert = (from data in targetingEventData
                                                              where
                                                                  data.Entity is LanguageTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Insert
                                                              select data).ToList();

            var targetingEventData_LanguageTargeting_Delete = (from data in targetingEventData
                                                              where
                                                                  data.Entity is LanguageTargeting &&
                                                                  data.ActionType == ObjectActionEnum.Delete
                                                              select data).ToList();

            foreach (var data in targetingEventData_LanguageTargeting_Insert)
            {
                var Language = (data.Entity as LanguageTargeting).GetDescription();
               
                builder.AppendLine(string.Format(rowTemplate, Language));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            builder.Clear();


            foreach (EntityEventData data in targetingEventData_LanguageTargeting_Delete)
            {
                var Language = (data.Entity as LanguageTargeting).GetDescription();

                builder.AppendLine(string.Format(rowTemplate, Language));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateDeleted.Replace("@Fields", fields));
            }

            return template.ToString();

        }
        private string GetBidConfigChangesDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />AdGroup Bid Configs Added<br /><table> <tr> <td>ID</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Bid</td> </tr>@Fields</table>";
            const string templateUpdated = @"<br />AdGroup Bid Configs Updated<br /><table> <tr> <td>ID</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Bid</td> </tr>@Fields</table>";
            const string templateDeleted = @"<br />AdGroup Bid Configs Deleted<br /><table> <tr> <td>ID</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Bid</td> </tr>@Fields</table>";

            string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";


            var targetingEventData_BidConfigs_Insert = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupBidConfig &&
                                                            data.ActionType == ObjectActionEnum.Insert
                                                        select data).ToList();

            var targetingEventData_BidConfigs_Update = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupBidConfig &&
                                                            data.ActionType == ObjectActionEnum.Update
                                                        select data).ToList();

            var targetingEventData_BidConfigs_Delete = (from data in targetingEventData
                                                        where
                                                            data.Entity is AdGroupBidConfig &&
                                                            data.ActionType == ObjectActionEnum.Delete
                                                        select data).ToList();

            foreach (var data in targetingEventData_BidConfigs_Insert)
            {
                var campaignBidConfig = (data.Entity as AdGroupBidConfig);

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.AppSite.Name, campaignBidConfig.SubPublisherId, campaignBidConfig.Bid));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_BidConfigs_Update)
            {
                var campaignBidConfigObj = (data.Entity as AdGroupBidConfig);

                var campaignBidConfig = adGroupObj.GetCampaignBidConfigs().Where(x => x.ID == campaignBidConfigObj.ID).FirstOrDefault();

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.AppSite.Name, campaignBidConfig.SubPublisherId, campaignBidConfig.Bid));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateUpdated.Replace("@Fields", fields));
            }
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_BidConfigs_Delete)
            {
                //data.Entity = adGroupObj.CampaignBidConfigs.Where(x => x.ID == (data.Entity as CampaignBidConfig).ID);
                //    deletedText += ("," + (data.Entity as CampaignBidConfig).GetDescription());
                var campaignBidConfigObj = (data.Entity as AdGroupBidConfig);

                var campaignBidConfig = adGroupObj.GetCampaignBidConfigs(true).Where(x => x.ID == campaignBidConfigObj.ID).FirstOrDefault();

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.AppSite.Name, campaignBidConfig.SubPublisherId, campaignBidConfig.Bid));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateDeleted.Replace("@Fields", fields));
            }

            return template.ToString();
        }
        private string GetAudianceChangesDesc(IList<EntityEventData> targetingEventData, AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />Audiance Added<br />  <table> <tr> <td>Query</td> </tr>@Fields</table>";
            const string templateUpdated = @"<br />Audiance Updated<br /><table> <tr> <td>Query</td> </tr>@Fields</table>";
            const string templateDeleted = @"<br />Audiance Deleted<br /><table> <tr> <td>Query</td> </tr>@Fields</table>";

            string rowTemplate = @"<tr><td>{0}</td></tr>";


            var targetingEventData_Audiance_Insert = (from data in targetingEventData
                                                      where
                                                          data.Entity is AudienceSegmentTargeting &&
                                                          data.ActionType == ObjectActionEnum.Insert
                                                      select data).ToList();

            var targetingEventData_Audiance_Update = (from data in targetingEventData
                                                      where
                                                          data.Entity is AudienceSegmentTargeting &&
                                                          data.ActionType == ObjectActionEnum.Update
                                                      select data).ToList();

            var targetingEventData_Audiance_Delete = (from data in targetingEventData
                                                      where
                                                          data.Entity is AudienceSegmentTargeting &&
                                                          data.ActionType == ObjectActionEnum.Delete
                                                      select data).ToList();

            foreach (var data in targetingEventData_Audiance_Insert)
            {
                var AudienceSegment = (data.Entity as AudienceSegmentTargeting).RulesJson;
                var groupJson = (data.Entity as AudienceSegmentTargeting).GetRulesJsonForGroup(AudienceSegment);
                builder.AppendLine(string.Format(rowTemplate, AudienceSegmentTargeting.computed(groupJson)));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_Audiance_Update)
            {
                var AudienceSegment = (data.Entity as AudienceSegmentTargeting).RulesJson;
                var groupJson = (data.Entity as AudienceSegmentTargeting).GetRulesJsonForGroup(AudienceSegment);
                builder.AppendLine(string.Format(rowTemplate, AudienceSegmentTargeting.computed(groupJson)));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateUpdated.Replace("@Fields", fields));
            }
            builder.Clear();
            foreach (EntityEventData data in targetingEventData_Audiance_Delete)
            {


                builder.AppendLine(string.Format(rowTemplate, "()"));

            }
            fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateDeleted.Replace("@Fields", fields));
            }

            return template.ToString();
        }

        ////private string computed(string json)
        //{
        //    group results = JsonConvert.DeserializeObject<group>(json);

        //    string item = "";
        //    item += "(";
        //    foreach (child child in results.rules)
        //    {
        //        item += "(";
        //        child last = child.group.rules.Last();

        //        foreach (child Subchild in child.group.rules)
        //        {
        //            item += Subchild.Name + " " + Subchild.condition + " " + Subchild.Price;


        //            if (!Subchild.Equals(last))
        //            {
        //                item += " " + child.group.Operator + " ";
        //            }

        //        }
        //        item += ")";
        //        last = results.rules.Last();

        //        if (!child.Equals(last))
        //        {
        //            item += " " + results.Operator + " ";
        //        }


        //    }
        //    item += ")";


        //    return item;

        //}
        private string GetBidConfigDesc(AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var deletedText = string.Empty;
            var InsertText = string.Empty;
            var updatedText = string.Empty;
            var template = new StringBuilder();
            const string templateAdded = @"<br />AdGroup Bid Configs<br /><table> <tr> <td>ID</td> <td>AppSite Name</td> <td>Sub Publisher</td>  <td>Bid</td> </tr>@Fields</table>";


            string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";


            var targetingEventData_BidConfigs_Insert = (from data in adGroupObj.CampaignBidConfigs
                                                        where
                                                            data is AdGroupBidConfig

                                                        select data).ToList();


            foreach (var data in targetingEventData_BidConfigs_Insert)
            {
                var campaignBidConfig = (data as AdGroupBidConfig);

                builder.AppendLine(string.Format(rowTemplate, campaignBidConfig.ID, campaignBidConfig.AppSite.Name, campaignBidConfig.SubPublisherId, campaignBidConfig.Bid));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();



            return template.ToString();
        }

        private string GetAdGroupChanges(EntityEventData eventData)
        {
            const string template = @"<br />
AdGroup changes
<br /><table>
 <tr> <td>ID</td> <td>Field Name</td> <td>Old Value</td> <td>New Value</td> </tr>
@Fields</table>";
            var Fields = GetDirtyDesc(eventData);
            if (string.IsNullOrWhiteSpace(Fields))
                return string.Empty;
            else
            {
                return template.Replace("@Fields", Fields);
            }
        }
        private string GetAdsDesc(IList<AdCreative> ads)
        {
            var builder = new StringBuilder();
            var adGroupIds = new List<int>();

            const string template = "<p>Ad:<b>{0}</b></p><table><tr><td>ID</td> <td>Field Name</td><td>Value</td></tr>{1}</table>";

            foreach (var adCreative in ads)
            {
                var adObj = adCreativeRepository.Get(adCreative.ID);// campaignObj.GetGroupAds(adGroupObj).FirstOrDefault(ad => ad.ID == adCreative.ID);
                var adGroupObj = adObj.Group;
                if (!adGroupIds.Contains(adGroupObj.ID))
                {
                    builder.Append(GetGroupDesc(adGroupObj));
                    builder.Append("<p><b>Ads: </b></p>");
                    adGroupIds.Add(adGroupObj.ID);
                }
                var adName = adObj.GetDescription();
                var changes = GetAdDesc(adObj);
                builder.Append(string.Format(template, adName, changes));
            }
            return builder.ToString();
        }
        private string GetCostElementDesc(AdGroup adGroupObj)
        {
            var builder = new StringBuilder();
            var template = new StringBuilder();
            const string templateAdded = @"<br />AdGroup Cost Elements<br /><table> <tr> <td>ID</td> <td>Beneficiary</td> <td>FromDate</td>  <td>ToDate</td> <td>Value</td>  </tr>@Fields</table>";


            string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";


            var targetingEventData_CostElements_Insert = adGroupObj.CostElements.ToList();

            foreach (var data in targetingEventData_CostElements_Insert)
            {

                builder.AppendLine(string.Format(rowTemplate, data.ID, data.Beneficiary, GetString(data.FromDate), data.ToDate.HasValue ? GetString(data.ToDate.Value) : string.Empty, data.Value));
            }
            var fields = builder.ToString();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                template.AppendLine(templateAdded.Replace("@Fields", fields));
            }
            // string.IsNullOrWhiteSpace(fields) ? string.Empty : templateAdded.Replace("@Fields", fields);
            builder.Clear();



            return template.ToString();
        }

        #endregion
        #region AppSite
        private string GetAppSiteDesc(AppSite appSite)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";

            //App Site Description

            //App/Site Name
            builder.Append(string.Format(rowTemplate, index, "Name", appSite.Name));
            index++;

            //App/Site Type
            builder.Append(string.Format(rowTemplate, index, "Type", appSite.Type.GetDescription()));
            index++;

            //App/Site Status
            builder.Append(string.Format(rowTemplate, index, "Status", appSite.Status.GetDescription()));
            index++;

            //Available in market checkbox
            builder.Append(string.Format(rowTemplate, index, "Available in market", appSite.IsPublished));
            index++;

            //App/Site URL
            var url = appSite.GetURL();
            builder.Append(string.Format(rowTemplate, index, "URL", url));
            index++;

            return builder.ToString();
        }
        #endregion
        #region Account
        protected string GetLoggedInUser()
        {
            if (
                (OperationContext.Current.UserInfo<AdFalconUserInfo>() != null) &&
                (OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.HasValue))
            {
                if (OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId > 0)
                    return userRepository.Get(OperationContext.Current.UserInfo<AdFalconUserInfo>().OriginalUserId.Value).GetName();
            }
            else
            {
                return string.Empty;
            }
            return string.Empty;
        }
        protected string GetExtra(EventArgsBase args)
        {
            var result = string.Empty;
            return result;
        }
        protected string GetAccountBankInfo(EntityEventData eventData, EntityEventData eventOldData)
        {
            var index = 1;
            var builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";

            var bankObj = eventData.Entity as BankAccountPaymentDetails;
            var bankOldObj = eventOldData.Entity as BankAccountPaymentDetails;
            //account Bank Description
            //Bank Name

            if (!bankOldObj.BankName.Equals(bankObj.BankName))
            {
                builder.Append(string.Format(rowTemplate, index, "Bank Name", bankOldObj.BankName, bankObj.BankName));
                index++;
            }

            //Bank Address
            if (!bankOldObj.BankAddress.Equals(bankObj.BankAddress))
            {
                builder.Append(string.Format(rowTemplate, index, "Bank Address", bankOldObj.BankAddress, bankObj.BankAddress));
                index++;
            }

            //Beneficiary Name
            if (!bankOldObj.BeneficiaryName.Equals(bankObj.BeneficiaryName))
            {
                builder.Append(string.Format(rowTemplate, index, "Beneficiary Name", bankOldObj.BeneficiaryName, bankObj.BeneficiaryName));
                index++;
            }

            //Recipient Account Number
            if ((string.IsNullOrWhiteSpace(bankOldObj.RecipientAccountNumber)) || (!bankOldObj.RecipientAccountNumber.Equals(bankObj.RecipientAccountNumber)))
            {
                builder.Append(string.Format(rowTemplate, index, "RecipientAccountNumber", bankOldObj.RecipientAccountNumber, bankObj.RecipientAccountNumber));
                index++;
            }

            //SWIFT
            if (!bankOldObj.SWIFT.Equals(bankObj.SWIFT))
            {
                builder.Append(string.Format(rowTemplate, index, "SWIFT", bankOldObj.SWIFT, bankObj.SWIFT));
                index++;
            }
            //Is Default
            if (!bankOldObj.IsDefault != bankObj.IsDefault)
            {
                builder.Append(string.Format(rowTemplate, index, "Is Default", bankOldObj.IsDefault, bankObj.IsDefault));
                index++;
            }
            return builder.ToString();
        }



        #endregion
        #region New Payment Details
        protected string GetPaymentAccountInfo(EntityEventData eventData)
        {
            const string tableTemplate = @"{0}</br><table><tr><td>ID</td><td>Name</td><td>Value</td></tr>{1}</table>";
            if (eventData.Entity is BankAccountPaymentDetails)
                return string.Format(tableTemplate, "Bank", GetPaymentAccountBankInfo(eventData));
            if (eventData.Entity is PayPalAccountPaymentDetails)
                return string.Format(tableTemplate, "PayPal", GetPaymentAccountPayPalInfo(eventData));
            return string.Empty;
        }
        protected string GetPaymentAccountPayPalInfo(EntityEventData eventData)
        {
            var index = 1;
            var builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            var paypalObj = eventData.Entity as PayPalAccountPaymentDetails;
            //account paypal Description
            //User Name
            builder.Append(string.Format(rowTemplate, index, "User Name", paypalObj.UserName));
            index++;

            return builder.ToString();
        }
        protected string GetPaymentAccountBankInfo(EntityEventData eventData)
        {
            var index = 1;
            var builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";

            var bankObj = eventData.Entity as BankAccountPaymentDetails;
            //account Bank Description
            //Bank Name
            builder.Append(string.Format(rowTemplate, index, "Bank Name", bankObj.BankName));
            index++;

            //Bank Address
            builder.Append(string.Format(rowTemplate, index, "Bank Address", bankObj.BankAddress));
            index++;

            //Beneficiary Name
            builder.Append(string.Format(rowTemplate, index, "Beneficiary Name", bankObj.BeneficiaryName));
            index++;

            //Recipient Account Number
            builder.Append(string.Format(rowTemplate, index, "RecipientAccountNumber", bankObj.RecipientAccountNumber));
            index++;

            //SWIFT
            builder.Append(string.Format(rowTemplate, index, "SWIFT", bankObj.SWIFT));
            index++;
            //Is Default
            builder.Append(string.Format(rowTemplate, index, "Is Default", bankObj.IsDefault));
            index++;
            return builder.ToString();
        }
        #endregion
        protected string GetAccountDesc(Account account)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";

            //account Description

            //account Email
            builder.Append(string.Format(rowTemplate, index, "Email", account.PrimaryUser.EmailAddress));
            index++;

            //account Name
            builder.Append(string.Format(rowTemplate, index, "Name", account.GetName()));
            index++;

            //account Phone
            if (!string.IsNullOrWhiteSpace(account.PrimaryUser.Phone))
            {
                builder.Append(string.Format(rowTemplate, index, "Phone", account.PrimaryUser.Phone));
                index++;
            }

            //account Company
            builder.Append(string.Format(rowTemplate, index, "Company", account.PrimaryUser.Company));
            index++;

            //account Country
            builder.Append(string.Format(rowTemplate, index, "Country", account.PrimaryUser.Country.GetDescription()));
            index++;

            //account Language
            builder.Append(string.Format(rowTemplate, index, "Language", account.PrimaryUser.Language.GetDescription()));
            index++;

            return builder.ToString();
        }

        protected string GetPaymentDetailDesc(Payment payment)
        {

            if (payment is PaymentCash)
            {
                return "Cash";
            }
            if (payment is PaymentCheck)
            {
                var obj = payment as PaymentCheck;
                return string.Format("Using Check for '{0}' from bank '{1}'", obj.BeneficiaryName, obj.SystemPaymentDetail.BankName);
            }
            if (payment is PaymentPaypal)
            {
                var obj = payment as PaymentPaypal;
                return string.Format("Using '{0}'", GetPaymentDetailAccountDesc(obj.AccountPaymentDetail));
            }
            if (payment is PaymentWire)
            {
                var obj = payment as PaymentWire;
                return string.Format("Using '{0}'", GetPaymentDetailAccountDesc(obj.AccountPaymentDetail));
            }
            return string.Empty;
        }

        protected string GetFundPaymentDetailDesc(AccountFundTransHistory accountFundTransHistory)
        {
            if (accountFundTransHistory is AccountFundTransHistoryWire)
            {
                var obj = accountFundTransHistory as AccountFundTransHistoryWire;
                return string.Format("Using {0}", GetPaymentDetailAccountDesc(obj.AccountPaymentDetail));
            }
            if (accountFundTransHistory is AccountFundTransHistoryCash)
            {
                return "Cash";
            }
            if (accountFundTransHistory is AccountFundTransHistoryCheck)
            {
                var obj = accountFundTransHistory as AccountFundTransHistoryCheck;
                return string.Format("Using Check from '{0}' bank '{1}'", obj.IssuerName, obj.IssuerBankName);
            }
            if (accountFundTransHistory is AccountFundTransHistoryPaypal)
            {
                var obj = accountFundTransHistory as AccountFundTransHistoryPaypal;
                return string.Format("Using {0}", GetPaymentDetailAccountDesc(obj.AccountPaymentDetail));
            }

            return string.Empty;
        }
        protected string GetPaymentDetailAccountDesc(AccountPaymentDetails obj)
        {
            var result = string.Empty;
            switch (obj.AccountType)
            {
                case PayemntAccountType.Bank:
                    {
                        result = string.Format(" bank '{0}' account", obj.GetDescription());
                        break;
                    }
                case PayemntAccountType.PayPal:
                    {
                        result = string.Format(" PayPal '{0}' account", obj.GetDescription());
                        break;
                    }
            }
            return result;
        }

        #endregion
        #endregion

        #region Implementation of ISubscriberHandler

        //public void HandleEvent<T>(EventArgsBase<T> args) where T : class, new()
        public virtual void HandleEvent(EventArgsBase args)
        {
            Framework.ApplicationContext.Instance.Logger.InfoFormat("{0}:Email Sender handling Event {1}", "Event Broker", args.EventName);
            var emailBody = string.Empty;
            var subject = string.Empty;
            var bccEmailAddress = string.Empty;
            Dictionary<string, string> sendToEmails = GetSendToEmails(args.EventName);

            EmailInfo emailInfo;
            switch (args.EventName)
            {
                case EventNames.close_fund_transaction:
                    {
                        emailBody = CloseFund(args);
                        bccEmailAddress = Domain.Configuration.FinanceEmail;
                        break;
                    }
                case EventNames.Save_ReportScheduler:
                    {
                        emailBody = SaveReportScheduler(args);
                        break;
                    }
                case EventNames.LookUp_Save:
                    {
                        emailBody = SaveLookUp(args);
                        //Todo to be linked with lookup type
                        subject = ResourceManager.Instance.GetResource("EmailSubjectAdvertiser Save", "EventBroker_Emails", new CultureInfo("en-US"));
                        break;
                    }
                case EventNames.Save_Campaign:
                    {
                        emailBody = SaveCampaign(args);
                        break;
                    }
                case EventNames.Save_Targeting:
                    {
                        emailBody = SaveTargeting(args);
                        break;
                    }
                case EventNames.Save_Ad:
                    {
                        emailBody = SaveAd(args);
                        break;
                    }
                case EventNames.Delete_Campaign:
                    {
                        emailBody = DeleteCampaign(args);
                        break;
                    }
                case EventNames.Run_Campaign:
                    {
                        emailBody = RunCampaign(args);
                        break;
                    }
                case EventNames.Pause_Campaign:
                    {
                        emailBody = PauseCampaign(args);
                        break;
                    }

                case EventNames.Delete_AdGroup:
                    {
                        emailBody = DeleteAdGroup(args);
                        break;
                    }
                case EventNames.Run_AdGroup:
                    {
                        emailBody = RunAdGroup(args);
                        break;
                    }
                case EventNames.Pause_AdGroup:
                    {
                        emailBody = PauseAdGroup(args);
                        break;
                    }


                case EventNames.Delete_Ad:
                    {
                        emailBody = DeleteAd(args);
                        break;
                    }
                case EventNames.Run_Ad:
                    {
                        emailBody = RunAd(args);
                        break;
                    }
                case EventNames.Pause_Ad:
                    {
                        emailBody = PauseAd(args);
                        break;
                    }
                case EventNames.Save_App:
                    {
                        emailBody = SaveApp(args);
                        break;
                    }
                case EventNames.Delete_App:
                    {
                        emailBody = DeleteApp(args);
                        break;
                    }
                case EventNames.AppSiteApprove:
                    {
                        emailInfo = ApproveApp(args);
                        emailBody = emailInfo.Body;
                        subject = emailInfo.Subject;
                        break;
                    }
                case EventNames.Register_User:
                    {
                        emailBody = RegisterUser(args);
                        break;
                    }
                case EventNames.Update_Bank_Account_Info:
                    {
                        emailBody = UpdateBankAccountInfo(args);
                        break;
                    }
                case EventNames.Copy_Campaign:
                    {
                        emailBody = HandelCopyAd(args);
                        break;
                    }
                case EventNames.Copy_AdGroup:
                    {
                        emailBody = HandelCopyAd(args);
                        break;
                    }
                case EventNames.Copy_Ad:
                    {
                        emailBody = HandelCopyAd(args);
                        break;
                    }

                case EventNames.Add_Payment:
                    {
                        emailBody = HandelAddPayment(args);
                        break;
                    }
                case EventNames.Add_Fund:
                    {
                        string subjectou;
                        emailBody = HandelAddFund(args, out subjectou);
                        subject = subjectou;
                        break;
                    }
                case EventNames.Account_DSP_Request:
                    {
                        emailBody = HandelAccountDSPRequest(args);
                        break;
                    }

            }
            if (string.IsNullOrWhiteSpace(emailBody)) return;
            if (string.IsNullOrWhiteSpace(subject))
            {
                subject = ResourceManager.Instance.GetResource(string.Format(emailSubject, args.EventName), "EventBroker_Emails", new CultureInfo("en-US"));
            }
            var _mailSender = Framework.IoC.Instance.Resolve<IMailSender>();

            _mailSender.SendEmail("", "", sendToEmails, subject, emailBody, true, bccEmailAddress);

        }



        #region Handlers
        #region Admin

        private string HandelAccountDSPRequest(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is AccountDSPRequest)
                {
                    eventData = eventDataItem;
                    break;
                }
            }

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }
            if (eventData.ActionType != ObjectActionEnum.Insert)
                return string.Empty;
            AccountDSPRequest entity = null;

            entity = (eventData.Entity as AccountDSPRequest);




            var type = string.Empty;


            var emailTemplate = ResourceManager.Instance.GetResource("AccountDSPRequest", "EventBroker_Emails");

            emailBody = emailTemplate
                .Replace("@AccountDSPRequestEmail”", entity.EmailAddress)

                .Replace("@Extra", GetExtra(args))
                .Replace("@AccountDSPRequestName”", entity.FirstName + " " + entity.LastName)
            ;

            return emailBody;
        }

        private string HandelAddFund(EventArgsBase args , out string subjectout)
        {
            subjectout = string.Empty;
               var emailBody = string.Empty;
            IList<EntityEventData> eventData = args.Data.Select(item => item as EntityEventData).ToList();
            IList<AccountFundTransHistory> fundObjs =
                (from data in eventData
                 where data.Entity is AccountFundTransHistory
                 select data.Entity as AccountFundTransHistory).ToList();

            eventData.Select(item => item.Entity as Account).ToList();
            var fundObj = fundObjs.First();//fundTransRepository.Get(fundObjs.First().ID);
            var accountObj = accountRepository.Get(fundObj.AccountId);
            var accountSummaryObj = accountObj.AccountSummary;

            var accountName = accountObj.GetDescription();
            var currentAccountName = string.Empty;
            var acccurrent = _CurrentAccount;
            if(acccurrent!=null)
           currentAccountName = acccurrent.GetDescription();
            var accountTotal = FormatDecimal(accountSummaryObj.Funds);

            var type = string.Empty;

            if (fundObj.FundTransStatus.ID == AccountFundTransStatus.Committed.ID)
            {
                var emailTemplate = ResourceManager.Instance.GetResource("AddFundAdmin", "EventBroker_Emails");
                //emailTemplate =string.Format(emailTemplate.Replace("{name}", fundObj.).Replace("{email}", fundDto.Email).Replace("{phonenumber}", fundDto.PhoneNumber).Replace("{comment}", fundDto.Comment));
                emailBody = string.Format(emailTemplate, accountName, fundObj.Amount, accountTotal);
                emailBody = emailTemplate
                    .Replace("@CurrentAccountName", currentAccountName)
                    .Replace("@AccountName", accountName)
                    .Replace("@Type", GetFundPaymentDetailDesc(fundObj))
                    .Replace("@Fund", FormatDecimal(fundObj.Amount))
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@TotalFund", accountTotal);
            }

            if (fundObj.FundTransType != null && fundObj.FundTransType.ID == AccountFundTransType.OverBudgetRefund.ID)
            {
                subjectout = ResourceManager.Instance.GetResource("KafkaEventOverBudgetSubject", "EventBroker_Emails", new CultureInfo("en-US"));
            }
          
            return emailBody;
        }
        private string HandelAddPayment(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventData = args.Data.Select(item => item as EntityEventData).ToList();
            IList<Payment> fundObjs =
                (from data in eventData
                 where data.Entity is Payment
                 select data.Entity as Payment).ToList();

            eventData.Select(item => item.Entity as Account).ToList();
            var paymentObj = fundObjs.First(); //fundTransRepository.Get(fundObjs.First().ID);
            var accountObj = accountRepository.Get(paymentObj.Account.ID);
            var accountSummaryObj = accountObj.AccountSummary;

            var accountName = accountObj.GetDescription();
            var accCurr = _CurrentAccount;
            var currentAccountName = string.Empty;
                if (accCurr!=null)
             currentAccountName = _CurrentAccount.GetDescription();
            var accountTotal = FormatDecimal(accountSummaryObj.Earning);
            var emailTemplate = ResourceManager.Instance.GetResource("AddPayment", "EventBroker_Emails");
            emailBody = emailTemplate
                .Replace("@CurrentAccountName", currentAccountName)
                .Replace("@AccountName", accountName)
                .Replace("@Payment", FormatDecimal(paymentObj.Amount))
                .Replace("@Type", GetPaymentDetailDesc(paymentObj))
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@TotalEarnings", accountTotal);
            return emailBody;
        }

        #endregion
        #region Account
        private string RegisterUser(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is User)
                {
                    eventData = eventDataItem;
                    break;
                }
            }

            if (eventData == null)
            {

                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }

            if (Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountId.HasValue)
            {
                var accountObj = accountRepository.Get(Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountId.Value);

                //if (eventData.DirtyProperties.Any())
                {
                    var Fields = string.Empty;
                    var emailTemplate = string.Empty;
                    /*if (eventData.ActionType == ObjectActionEnum.Update)
                    {
                        emailTemplate = ResourceManager.Instance.GetResource("SaveApp_Old", "EventBroker_Emails");
                        Fields = GetDirtyDesc(eventData);
                    }
                    else*/
                    {
                        emailTemplate = ResourceManager.Instance.GetResource("RegisterUser", "EventBroker_Emails");
                        Fields = GetAccountDesc(accountObj);
                    }
                    emailBody = emailTemplate
                        .Replace("@Extra", GetExtra(args))
                        .Replace("@LoggedInUser", GetLoggedInUser())
                        .Replace("@Fields", Fields);
                }
            }
            return emailBody;
        }
        private string CloseFund(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventData = args.Data.Select(item => item as EntityEventData).ToList();
            IList<AccountFundTransHistory> fundObjs =
                (from data in eventData
                 where data.Entity is AccountFundTransHistory
                 select data.Entity as AccountFundTransHistory).ToList();

            eventData.Select(item => item.Entity as Account).ToList();
            var fundObj = fundTransRepository.Get(fundObjs.First().ID);
            var accountObj = accountRepository.Get(fundObj.AccountId);
            var accountSummaryObj = accountObj.AccountSummary;

            var accountName = accountObj.GetDescription();
            var accountTotal = accountSummaryObj.Funds.ToString();
            if (fundObj.FundTransStatus.ID == AccountFundTransStatus.Committed.ID)
            {
                var emailTemplate = ResourceManager.Instance.GetResource("AddFund", "EventBroker_Emails");
                //emailTemplate =string.Format(emailTemplate.Replace("{name}", fundObj.).Replace("{email}", fundDto.Email).Replace("{phonenumber}", fundDto.PhoneNumber).Replace("{comment}", fundDto.Comment));
                emailBody = string.Format(emailTemplate, accountName, fundObj.Amount, accountTotal);
                emailBody = emailTemplate
                    .Replace("@AccountName", accountName)
                    .Replace("@Fund", fundObj.Amount.ToString())
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@TotalFund", accountTotal);
            }
            return emailBody;
        }
        private string UpdateBankAccountInfo(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            /*var eventData = eventDataList.FirstOrDefault(item => item.Entity as AccountPaymentDetails != null && item.ActionType == ObjectActionEnum.Insert);
            var eventOldData = eventDataList.FirstOrDefault(item => item.Entity as AccountPaymentDetails != null && item.ActionType == ObjectActionEnum.Update);*/
            var eventData = eventDataList.Where(item => item.Entity as AccountPaymentDetails != null && item.ActionType == ObjectActionEnum.Insert).ToList();
            var eventOldData = eventDataList.Where(item => item.Entity as AccountPaymentDetails != null && item.ActionType == ObjectActionEnum.Update).ToList();


            if (((eventData == null) || (eventData.Count == 0)) && ((eventOldData == null) || (eventOldData.Count == 0)))
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }
            var entity = eventData.Count > 0
                             ? eventData.First().Entity as AccountPaymentDetails
                             : eventOldData.First().Entity as AccountPaymentDetails;
            var accountObj = accountRepository.Get(entity.Account.ID);
            var accountName = accountObj.GetDescription();
            var desc = string.Empty;
            if (eventData.Count > 0)
            {
                desc += "New:<br/>";
                foreach (var data in eventData)
                {
                    desc += GetPaymentAccountInfo(data);
                }
            }

            if (eventOldData.Count > 0)
            {
                desc += "Update:<br/>";

                foreach (var data in eventOldData)
                {
                    string entityDesc = "Bank";
                    if (data.Entity is PayPalAccountPaymentDetails)
                        entityDesc = "PayPal";
                    desc += string.Format(@"{0}<br /><table> <tr><td>ID</td><td>Field Name</td> <td>Old Value</td> <td>New Value</td> </tr>{1}</table>", entityDesc, GetDirtyDesc(data));
                }
            }
            //var desc =GetAccountBankInfo(eventData, eventOldData);
            var emailTemplate = ResourceManager.Instance.GetResource("UpdateBankAccountInfo", "EventBroker_Emails");


            if (Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().VATValue > 0)
            {
                //var accountObj = accountRepository.Get(Noqoush.Framework.OperationContext.Current.UserInfo<Noqoush.AdFalcon.Common.UserInfo.AdFalconUserInfo>().AccountId.Value);



                emailBody = emailTemplate
            .Replace("@AccountName", accountName)
            .Replace("@Extra", "Tax No:" + accountObj.TaxNo)
            .Replace("@LoggedInUser", GetLoggedInUser())
            .Replace("@Fields", desc);
            }
            else
            {

                emailBody = emailTemplate
            .Replace("@AccountName", accountName)
            .Replace("@Extra", GetExtra(args))
            .Replace("@LoggedInUser", GetLoggedInUser())
            .Replace("@Fields", desc);
            }
            return emailBody;
        }

        #endregion
        #region Campaign
        private bool HandleSaveCampaignEvent(EntityEventData eventData)
        {
            return ((IsDirty(eventData, "Budget")) ||
                    (IsDirty(eventData, "DailyBudget")) ||
                    (IsDirty(eventData, "StartDate")) ||
                    (IsDirty(eventData, "EndDate")));
        }
        private bool HandleSaveAdEvent(EntityEventData eventData)
        {
            return IsDirty(eventData, "Status") || IsDirty(eventData, "PausedStatus");
        }
        private bool HandleSaveAdActionValueEvent(AdActionValue adActionValue)
        {

            return adActionValue.AdCreative.Status == AdCreativeStatus.Active || adActionValue.AdCreative.Status == AdCreativeStatus.ActiveAdServer ||
                   adActionValue.AdCreative.Status == AdCreativeStatus.BudgetPaused ||
                   (adActionValue.AdCreative.PausedStatus != null &&
                    (adActionValue.AdCreative.PausedStatus == AdCreativeStatus.Active || adActionValue.AdCreative.PausedStatus == AdCreativeStatus.ActiveAdServer)) ||
                   (adActionValue.AdCreative.PausedStatus != null &&
                    adActionValue.AdCreative.PausedStatus == AdCreativeStatus.BudgetPaused);
        }
        private bool HandleSaveAppSiteEvent(EntityEventData eventData)
        {
            return IsDirty(eventData, "Status");
        }
        private string SaveCampaign(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is Campaign)
                {
                    eventData = eventDataItem;
                    break;
                }
            }
            /*IList<Campaign> campaigns = (from data in eventData
                                         where data.Entity is Campaign
                                         select data.Entity as Campaign).ToList();
            eventData.Select(item => item.Entity as Account).ToList();
            var campaignObj = campaigns.First();*/
            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }
            if (eventData.ActionType == ObjectActionEnum.Insert)
                return string.Empty;

            if (!HandleSaveCampaignEvent(eventData))
                return string.Empty;
            var campaignObj = campaignRepository.Get((eventData.Entity as IEntity<int>).ID);

            //check if this campaign have ads on it
            if (!campaignObj.IsContainsAds())
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("SaveCampaign", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AccountName", accountName)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@Fields", GetDirtyDesc(eventData));
            }
            return emailBody;
        }
        private string SaveAd(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventDataAdActionValue =
                eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdActionValue);
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            if ((eventData == null) && (eventDataAdActionValue == null))
            {
                return string.Empty;
            }

            AdCreative entity = null;
            if (eventData != null)
            {
                entity = (eventData.Entity as AdCreative);
            }
            else
            {
                entity = (eventDataAdActionValue.Entity as AdActionValue).AdCreative;
            }
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            var adGroupObj = campaignObj.GetGroups().FirstOrDefault(adGroup => adGroup.ID == entity.Group.ID);
            var adObj = campaignObj.GetGroupAds(adGroupObj).FirstOrDefault(ad => ad.ID == entity.ID);

            var groupDesc = string.Empty;
            groupDesc = GetCampaignGroupDesc(campaignObj, adGroupObj);

            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupName = adGroupObj.GetDescription();
            var adName = adObj.GetDescription();
            var changes = string.Empty;


            var emailTemplate = string.Empty;
            if (eventDataList.First().ActionType == ObjectActionEnum.Update)
            {
                if (eventData != null)
                {
                    if (!HandleSaveAdEvent(eventData))
                        return string.Empty;
                }
                else
                {
                    if (!HandleSaveAdActionValueEvent(eventDataAdActionValue.Entity as AdActionValue))
                        return string.Empty;
                }
                emailTemplate = ResourceManager.Instance.GetResource("SaveAd_Old", "EventBroker_Emails");
                changes = GetAdCreativeChanges(eventDataList);

            }
            else
            {
                emailTemplate = ResourceManager.Instance.GetResource("SaveAd_New", "EventBroker_Emails");
                changes = GetAdDesc(adObj);
            }
            emailBody = emailTemplate
                .Replace("@CampaignName", campaignName)
                .Replace("@AccountName", accountName)
                .Replace("@GroupName", groupName)
                .Replace("@AdName", adName)
                .Replace("@Campaign_Group_Info", groupDesc)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@Fields", changes);
            return emailBody;
        }

        private string SaveTargeting(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            EntityEventData bidConfigEventData = null;
            EntityEventData InventorySourceEventData = null;
            EntityEventData ImpressionMetricEventData = null;
            EntityEventData LanguageEventData = null;


            AdGroup adGroupObj = null;
            var targetingEventData = (from data in eventDataList where data.Entity is ITargetingBase select data).ToList();
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is AdGroup)
                {
                    eventData = eventDataItem;
                    adGroupObj = eventData.Entity as AdGroup;
                    break;
                }
            }
            var campaingBidConfigEventData = (from data in eventDataList where data.Entity is AdGroupBidConfig select data).ToList();
            foreach (var eventDataItem in campaingBidConfigEventData)
            {
                if (eventDataItem.Entity is AdGroupBidConfig)
                {
                    bidConfigEventData = eventDataItem;
                    adGroupObj = (bidConfigEventData.Entity as AdGroupBidConfig).AdGroup;
                    break;
                }
            }
            var AdGroupInventorySourceEventData = (from data in eventDataList where data.Entity is AdGroupInventorySource select data).ToList();
            foreach (var eventDataItem in AdGroupInventorySourceEventData)
            {
                if (eventDataItem.Entity is AdGroupInventorySource)
                {
                    InventorySourceEventData = eventDataItem;
                    adGroupObj = (InventorySourceEventData.Entity as AdGroupInventorySource).AdGroup;
                    break;
                }
            }
            var AdGroupImpressionMetricEventData = (from data in eventDataList where data.Entity is ImpressionMetricTargeting select data).ToList();
            foreach (var eventDataItem in AdGroupImpressionMetricEventData)
            {
                if (eventDataItem.Entity is ImpressionMetricTargeting)
                {
                    ImpressionMetricEventData = eventDataItem;
                    adGroupObj = (ImpressionMetricEventData.Entity as ImpressionMetricTargeting).AdGroup;
                    break;
                }
            }
            var AdGroupLanguageTargetingEventData = (from data in eventDataList where data.Entity is LanguageTargeting select data).ToList();
            foreach (var eventDataItem in AdGroupLanguageTargetingEventData)
            {
                if (eventDataItem.Entity is LanguageTargeting)
                {
                    LanguageEventData = eventDataItem;
                    adGroupObj = (LanguageEventData.Entity as LanguageTargeting).AdGroup;
                    break;
                }
            }


            if ((eventData == null))
            {
                //try get the group from targeting list
                foreach (var data in targetingEventData)
                {
                    adGroupObj = ((ITargetingBase)data.Entity).AdGroup;
                    if (adGroupObj != null)
                        break;
                    if (data.Entity is DeviceTargetingBase)
                    {
                        adGroupObj = ((DeviceTargetingBase)data.Entity).DeviceTargeting.AdGroup;
                        if (adGroupObj != null)
                            break;
                    }
                }
            }
            else if (adGroupObj != null)
            {
                adGroupObj = eventData.Entity as AdGroup;
            }
            if (adGroupObj == null)
            {
                return string.Empty;
            }
            var campaignItem = campaignRepository.Get(adGroupObj.Campaign.ID);
            adGroupObj = (campaignItem.GetGroups().Where(adGroup => adGroup.ID == adGroupObj.ID).FirstOrDefault());

            //check if this group have ads on it
            if (!adGroupObj.IsContainsAds())
            {
                return string.Empty;
            }
            var accountName = campaignItem.Account.GetDescription();
            var campaignName = campaignItem.GetDescription();
            var adGroupChanges = string.Empty;
            if (eventData != null)
            {
                eventData.Entity = adGroupObj;
                adGroupChanges = GetAdGroupChanges(eventData);
            }
            var targetingChanges = GetTargetingDesc(targetingEventData, adGroupObj);
            var targetingAudianceChanges = GetAudianceChangesDesc(targetingEventData, adGroupObj);
            var bidConfigChanges = GetBidConfigChangesDesc(campaingBidConfigEventData, adGroupObj);
            var InentorySourceChanges = GetInventorySourceChangesDesc(AdGroupInventorySourceEventData, adGroupObj);
            var ImpressionMetricChanges = GetImpressionMetricChangesDesc(AdGroupImpressionMetricEventData, adGroupObj);
            var LanguageTargetingChanges = GetLanguageTargetingChangesDesc(AdGroupLanguageTargetingEventData, adGroupObj);

            //if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("SaveTargeting", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AccountName", accountName)
                    .Replace("@AdGroupName", adGroupObj.GetDescription())
                    .Replace("@AdGroupFields", adGroupChanges)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@TargetingFields", targetingChanges + bidConfigChanges + InentorySourceChanges + targetingAudianceChanges + ImpressionMetricChanges + LanguageTargetingChanges);
            }
            return emailBody;
        }
        #endregion
        #region AppSite
        private string SaveApp(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is AppSite)
                {
                    eventData = eventDataItem;
                    break;
                }
            }

            if (eventData == null)
            {
                return string.Empty;
            }

            var appObj = appSiteRepository.Get((eventData.Entity as IEntity<int>).ID);

            var accountName = appObj.Account.GetDescription();
            var appName = appObj.GetDescription();
            //if (eventData.DirtyProperties.Any())
            {
                var Fields = string.Empty;
                var emailTemplate = string.Empty;
                if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.UpdatCollection)
                {
                    if (!HandleSaveAppSiteEvent(eventData))
                        return string.Empty;
                    emailTemplate = ResourceManager.Instance.GetResource("SaveApp_Old", "EventBroker_Emails");
                    Fields = GetDirtyDesc(eventData);
                }
                else
                {
                    emailTemplate = ResourceManager.Instance.GetResource("SaveApp_New", "EventBroker_Emails");
                    Fields = GetAppSiteDesc(appObj);
                }
                emailBody = emailTemplate
                    .Replace("@AppName", appName)
                    .Replace("@AccountName", accountName)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@Fields", Fields);
            }
            return emailBody;
        }

        private EmailInfo ApproveApp(EventArgsBase args)
        {
            var result = new EmailInfo();

            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is AppSite)
                {
                    eventData = eventDataItem;
                    break;
                }
            }

            if (eventData == null)
            {

                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }

            var appObj = appSiteRepository.Get((eventData.Entity as IEntity<int>).ID);

            var accountName = appObj.Account.GetDescription();
            var appName = appObj.GetDescription();
            var emailTemplate = string.Empty;

            if (appObj.Status.ID == (int)AppSiteStatusEnum.Active)
            {
                emailTemplate = ResourceManager.Instance.GetResource("ApproveAppSite", "EventBroker_Emails");
                result.Subject = ResourceManager.Instance.GetResource("ApproveAppSiteEmailSubject", "EventBroker_Emails",
                                                                      new CultureInfo("en-US"));
            }
            else
            {
                emailTemplate = ResourceManager.Instance.GetResource("RejectAppSite", "EventBroker_Emails");
                emailTemplate = emailTemplate.Replace("@Comments", appObj.LastAdminComment);
                result.Subject = ResourceManager.Instance.GetResource("RejectAppSiteEmailSubject", "EventBroker_Emails",
                                                                      new CultureInfo("en-US"));
            }
            result.Body = emailTemplate
                .Replace("@AppName", appName)
                .Replace("@AccountName", accountName)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser());
            return result;
        }

        #endregion
        #region Status

        #region Campaign
        private string RunCampaign(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads, isIncludeSubmitted: true);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignNames = GetCampaignNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("RunCampaign", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignNames", campaignNames)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string PauseCampaign(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);
            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();
            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            // var adGroupObj = campaignObj.GetGroups().FirstOrDefault(adGroup => adGroup.ID == entity.Group.ID);
            ;
            //var adObj = campaignObj.GetGroupAds(adGroupObj).FirstOrDefault(ad => ad.ID == entity.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }

            var accountName = campaignObj.Account.GetDescription();
            var campaignNames = GetCampaignNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("PauseCampaign", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignNames", campaignNames)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string DeleteCampaign(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);
            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            //var adGroupObj = campaignObj.GetGroups().FirstOrDefault(adGroup => adGroup.ID == entity.Group.ID); ;
            //var adObj = campaignObj.GetGroupAds(adGroupObj).FirstOrDefault(ad => ad.ID == entity.ID);

            //check if this campaign have ads on it
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignNames = GetCampaignNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            var emailTemplate = ResourceManager.Instance.GetResource("DeleteCampaign", "EventBroker_Emails");
            emailBody = emailTemplate
                .Replace("@CampaignNames", campaignNames)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@AccountName", accountName);
            return emailBody;
        }

        #endregion
        #region AdGroup
        private string RunAdGroup(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads, isIncludeSubmitted: true);
            if (ads.Count == 0)
            {
                return string.Empty;
            }

            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupNames = GetAdGroupNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("RunAdGroup", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AdGroupNames", groupNames)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string PauseAdGroup(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }

            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupNames = GetAdGroupNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("PauseAdGroup", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AdGroupNames", groupNames)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string DeleteAdGroup(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupNames = GetAdGroupNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            var emailTemplate = ResourceManager.Instance.GetResource("DeleteAdGroup", "EventBroker_Emails");
            emailBody = emailTemplate
                .Replace("@CampaignName", campaignName)
                .Replace("@AdGroupNames", groupNames)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@AccountName", accountName);
            return emailBody;
        }

        #endregion
        #region Ad
        private string RunAd(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            var groupObj = campaignObj.GetGroups().FirstOrDefault(group => group.ID == entity.Group.ID);

            //check if this campaign have ads on it
            ads = GetActiveAds(ads, isIncludeSubmitted: true);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupName = groupObj.GetDescription();
            var AdsName = GetAdNames(ads);

            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("RunAd", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AdGroupName", groupName)
                    .Replace("@AdNames", AdsName)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string PauseAd(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            var groupObj = campaignObj.GetGroups().FirstOrDefault(group => group.ID == entity.Group.ID);

            //check if this campaign have ads on it
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupName = groupObj.GetDescription();
            var AdsName = GetAdNames(ads);

            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("PauseAd", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignName", campaignName)
                    .Replace("@AdGroupName", groupName)
                    .Replace("@AdNames", AdsName)
                    .Replace("@Extra", GetExtra(args))
                    .Replace("@LoggedInUser", GetLoggedInUser())
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }
        private string DeleteAd(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);
            var groupObj = campaignObj.GetGroups().FirstOrDefault(group => group.ID == entity.Group.ID);

            //check if one of the ads is active
            ads = GetActiveAds(ads);
            if (ads.Count == 0)
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var groupName = groupObj.GetDescription();
            var AdsName = GetAdNames(ads);

            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            var emailTemplate = ResourceManager.Instance.GetResource("DeleteAd", "EventBroker_Emails");
            emailBody = emailTemplate
                .Replace("@CampaignName", campaignName)
                .Replace("@AdGroupName", groupName)
                .Replace("@AdNames", AdsName)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@AccountName", accountName);
            return emailBody;
        }

        #endregion
        #region AppSite
        private string DeleteApp(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AppSite);
            IList<AppSite> apps = (from data in eventDataList
                                   where data.Entity is AppSite
                                   select data.Entity as AppSite).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));

            }

            var entity = (eventData.Entity as AppSite);
            var appObj = appSiteRepository.Get(entity.ID);


            var accountName = appObj.Account.GetDescription();
            var appNames = GetAppSiteNames(apps);
            var emailTemplate = ResourceManager.Instance.GetResource("DeleteApp", "EventBroker_Emails");
            emailBody = emailTemplate
                .Replace("@AppSiteNames", appNames)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@AccountName", accountName);
            return emailBody;
        }

        #endregion
        #region Helpers

        private bool IsDownloadAction(int adActionId)
        {
            return (adActionId == (int)AdActionTypeIds.DownloadiPhoneApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadiPadApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadiOSUniversalApplication ||
                    adActionId == (int)AdActionTypeIds.DownloadAndroidApplication);
        }

        private string GetCampaignNames(IList<AdCreative> Ads)
        {
            var builder = new StringBuilder();
            var campaignIds = new List<int>();
            /* foreach (var adCreative in Ads)
             {
                 if (!campaignIds.Contains(adCreative.Group.Campaign.ID))
                     campaignIds.Add(adCreative.Group.Campaign.ID);
             }
             foreach (var campaignId in campaignIds)
             {
                 var campaignObj = campaignRepository.Get(campaignId);
                 builder.AppendFormat(OrderedListTemplate, campaignObj.GetDescription());
             }*/

            foreach (var adCreative in Ads)
            {
                if (!campaignIds.Contains(adCreative.Group.Campaign.ID))
                {
                    var campaignId = adCreative.Group.Campaign.ID;
                    var campaignObj = campaignRepository.Get(campaignId);
                    builder.AppendFormat(OrderedListTemplate, campaignObj.GetDescription());
                    campaignIds.Add(campaignId);
                }
                builder.AppendFormat(OrderedListItemTemplate, string.Format("{0}({1})", adCreative.GetDescription(), adCreative.Status.Name));
            }
            /*foreach (var campaignId in campaignIds)
            {
                var campaignObj = campaignRepository.Get(campaignId);
                builder.AppendFormat(OrderedListTemplate, campaignObj.GetDescription());
            }*/


            return string.Format(OrderedListTemplate, builder.ToString());
        }
        private string GetAdGroupNames(IList<AdCreative> Ads)
        {
            var builder = new StringBuilder();
            var adGroupIds = new List<int>();
            foreach (var adCreative in Ads)
            {
                if (!adGroupIds.Contains(adCreative.Group.ID))
                {
                    builder.AppendFormat(OrderedListTemplate, adCreative.Group.GetDescription());
                    adGroupIds.Add(adCreative.Group.ID);
                }
                builder.AppendFormat(OrderedListItemTemplate, string.Format("{0}({1})", adCreative.GetDescription(), adCreative.Status.Name));
            }
            return string.Format(OrderedListTemplate, builder.ToString());
        }
        private string GetAdNames(IList<AdCreative> Ads)
        {
            var builder = new StringBuilder();
            var adGroupIds = new List<int>();
            foreach (var adCreative in Ads)
            {
                builder.AppendFormat(OrderedListItemTemplate, string.Format("{0}({1})", adCreative.GetDescription(), adCreative.Status.Name));
            }
            return string.Format(OrderedListTemplate, builder.ToString());
        }
        private string GetAppSiteNames(IList<AppSite> appSites)
        {
            var builder = new StringBuilder();
            foreach (var appsite in appSites)
            {
                builder.AppendFormat(OrderedListItemTemplate, appsite.GetDescription());
            }
            return string.Format(OrderedListTemplate, builder.ToString());
        }
        #endregion
        #endregion
        #region Copy
        /* private string CopyCampaign(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            var eventData = eventDataList.FirstOrDefault(eventDataItem => eventDataItem.Entity is AdCreative);

            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();

            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
                
            }

            var entity = (eventData.Entity as AdCreative);
            var campaignObj = campaignRepository.Get(entity.Group.Campaign.ID);

            //check if this campaign have ads on it
            if (!campaignObj.IsContainsAds())
            {
                return string.Empty;
            }
            var accountName = campaignObj.Account.GetDescription();
            var campaignNames = GetCampaignNames(ads);
            //var groupName = adGroupObj.GetDescription();
            //var adName = adObj.GetDescription();
            if (eventData.DirtyProperties.Any())
            {
                var emailTemplate = ResourceManager.Instance.GetResource("RunCampaign", "EventBroker_Emails");
                emailBody = emailTemplate
                    .Replace("@CampaignNames", campaignNames)
                    .Replace("@AccountName", accountName);
            }
            return emailBody;
        }

        private string CopyAd(EventArgsBase args)
        {
            return SaveAd(args);
        }
        private string CopyAdGroup(EventArgsBase args)
        {
            return HandelCopyAd(args);
        }*/

        private string HandelCopyAd(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            IList<AdCreative> ads = (from data in eventDataList
                                     where data.Entity is AdCreative
                                     select data.Entity as AdCreative).ToList();
            if (ads.Count == 0)
            {
                return string.Empty;

            }

            AdCreative entity = ads.First();
            var campaignObj = adCreativeRepository.Get(entity.ID).Group.Campaign;
            var accountName = campaignObj.Account.GetDescription();
            var campaignName = campaignObj.GetDescription();
            var emailTemplate = ResourceManager.Instance.GetResource("CopyAdGroup", "EventBroker_Emails");
            var changes = GetCampaignDesc(campaignObj) + GetAdsDesc(ads);

            emailBody = emailTemplate
                .Replace("@CampaignName", campaignName)
                .Replace("@AccountName", accountName)
                .Replace("@Extra", GetExtra(args))
                .Replace("@LoggedInUser", GetLoggedInUser())
                .Replace("@Ads", changes);
            return emailBody;
        }

        #endregion


        #region report schduler
        private bool HandleReportSchedulervent(EntityEventData eventData)
        {
            return ((IsDirty(eventData, "Name")) ||
                    (IsDirty(eventData, "EmailSubject")) ||
                    (IsDirty(eventData, "StartDate")) ||
                       (IsDirty(eventData, "IsActive")) ||

                    (IsDirty(eventData, "EndDate")));
        }

        private string SaveReportScheduler(EventArgsBase args)
        {
            var emailBody = string.Empty;
            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is ReportScheduler)
                {
                    eventData = eventDataItem;
                    break;
                }
            }
            /*IList<Campaign> campaigns = (from data in eventData
                                         where data.Entity is Campaign
                                         select data.Entity as Campaign).ToList();
            eventData.Select(item => item.Entity as Account).ToList();
            var campaignObj = campaigns.First();*/
            if (eventData == null)
            {
                throw new Exception(string.Format(_eventArgsMissingForEvent, args.EventName));
            }


            {
                var Fields = string.Empty;
                var emailTemplate = string.Empty;

                var repObj = ReportSchedulerRepository.Get((eventData.Entity as IEntity<int>).ID);


                var accountName = repObj.Account.GetDescription();
                var repName = repObj.Name;

                if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.UpdatCollection)
                {
                    if (!HandleReportSchedulervent(eventData))
                        return string.Empty;
                    emailTemplate = ResourceManager.Instance.GetResource("UpdateReportScheduler", "EventBroker_Emails");
                    Fields = GetDirtyDesc(eventData);
                }
                else
                {
                    emailTemplate = ResourceManager.Instance.GetResource("AddReportScheduler", "EventBroker_Emails");
                    Fields = GetReportSchedulerDesc(repObj);
                }
                emailBody = emailTemplate
       .Replace("@Name", repName)
       .Replace("@AccountName", accountName)
       .Replace("@Extra", GetExtra(args))
       .Replace("@LoggedInUser", GetLoggedInUser())
       .Replace("@Fields", Fields);


            }
            return emailBody;
        }


        private string GetReportSchedulerDesc(ReportScheduler reportSchdule)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";

            //App Site Description

            //App/Site Name
            builder.Append(string.Format(rowTemplate, index, "Name", reportSchdule.Name));
            index++;

            //App/Site Type
            builder.Append(string.Format(rowTemplate, index, "Recurrence", Enum.GetName(typeof(RecurrenceType), reportSchdule.RecurrenceType)));
            index++;
            //App/Site Type
            builder.Append(string.Format(rowTemplate, index, "Report Type", Enum.GetName(typeof(ReportSectionType), reportSchdule.ReportSectionType)));
            index++;
            //App/Site Status
            builder.Append(string.Format(rowTemplate, index, "Start Date", GetString(reportSchdule.StartDate)));
            index++;
            var endDate = "";
            if (reportSchdule.EndDate.HasValue)
            {
                endDate = GetString(reportSchdule.EndDate);

            }
            //Available in market checkbox
            builder.Append(string.Format(rowTemplate, index, "End Date", endDate));
            index++;

            builder.Append(string.Format(rowTemplate, index, "Email Subject", reportSchdule.EmailSubject));
            index++;

            return builder.ToString();
        }
        #endregion

        #region  LookUp
        private bool HandleAdvertiserevent(EntityEventData eventData)
        {
            return (
                    (IsDirty(eventData, "DomainURL"))


                   );
        }

        private string SaveLookUp(EventArgsBase args)
        {
            var emailBody = string.Empty;

            IList<EntityEventData> eventDataList = args.Data.Select(item => item as EntityEventData).ToList();
            EntityEventData eventData = null;
            foreach (var eventDataItem in eventDataList)
            {
                if (eventDataItem.Entity is Advertiser)
                {
                    eventData = eventDataItem;
                    emailBody = HandelAdvertiser(eventData).Replace("@Extra", GetExtra(args)).Replace("@LoggedInUser", GetLoggedInUser());
                    break;
                }
                //if (eventDataItem.Entity is CreativeVendor)
                //{
                //    eventData = eventDataItem;
                //    emailBody = HandelCreativeVendor(eventData).Replace("@Extra", GetExtra(args)).Replace("@LoggedInUser", GetLoggedInUser());
                //    break;
                //}
            }

            return emailBody;

        }
        private string HandelAdvertiser(EntityEventData eventData)
        {
            var Fields = string.Empty;
            var emailTemplate = string.Empty;
            var repObj = AdvertiserRepository.Get((eventData.Entity as IEntity<int>).ID);
            var repName = repObj.Name.Value;

            if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.UpdatCollection)
            {
                if (!HandleAdvertiserevent(eventData))
                    return string.Empty;
                emailTemplate = ResourceManager.Instance.GetResource("UpdateAdvertiser", "EventBroker_Emails");
                Fields = GetDirtyDesc(eventData);
            }
            else
            {
                emailTemplate = ResourceManager.Instance.GetResource("AddAdvertiser", "EventBroker_Emails");
                Fields = GetAdvertiserDesc(repObj);
            }
            return emailTemplate.Replace("@Fields", Fields).Replace("@Name", repName);

        }

        private string GetAdvertiserDesc(Advertiser Obj)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            foreach (var val in Obj.Name.Values)
            {
                builder.Append(string.Format(rowTemplate, index, val.Culture, val.Value));
                index++;
            }

            builder.Append(string.Format(rowTemplate, index, "Domain URL", Obj.DomainURL));
            index++;


            return builder.ToString();
        }
        private string GetCreativeVendorDesc(CreativeVendor Obj)
        {
            var index = 1;
            StringBuilder builder = new StringBuilder();
            const string rowTemplate = @"<tr><td>{0}</td><td>{1}</td></tr>";

            foreach (var val in Obj.Keywords)
            {
                builder.Append(string.Format(rowTemplate, index, val.Keyword));
                index++;
            }

            return builder.ToString();
        }

        private string HandelCreativeVendor(EntityEventData eventData)
        {
            var Fields = string.Empty;
            var emailTemplate = string.Empty;
            var repObj = CreativeVendorRepository.Get((eventData.Entity as IEntity<int>).ID);
            var repName = repObj.Name.Value;

            if (eventData.ActionType == ObjectActionEnum.Update || eventData.ActionType == ObjectActionEnum.UpdatCollection)
            {
                emailTemplate = ResourceManager.Instance.GetResource("UpdateCreativeVendor", "EventBroker_Emails");
            }
            else
            {
                emailTemplate = ResourceManager.Instance.GetResource("AddCreativeVendor", "EventBroker_Emails");
            }

            Fields = GetCreativeVendorDesc(repObj);

            return emailTemplate.Replace("@Fields", Fields).Replace("@Name", repName);

        }
        #endregion

        #endregion


        #endregion

    }

    internal class EventEmails
    {
        public string EventName { get; set; }

        public string Emails { get; set; }

        public string[] EmailsList
        {
            get
            {
                if (!string.IsNullOrEmpty(Emails))
                {
                    return Emails.Split(';').Where(p => !string.IsNullOrEmpty(p)).ToArray();
                }

                return new string[0];
            }
        }

        public bool SendToDefault { get; set; }
    }

    internal class EmailInfo
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Address { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
    }
}