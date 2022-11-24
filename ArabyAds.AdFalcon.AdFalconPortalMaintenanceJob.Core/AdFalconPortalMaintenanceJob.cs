using ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule;
using ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Utilities;
using ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Events;
using ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Video;
using ArabyAds.AdFalcon.Domain.Common.Model.Core;
using ArabyAds.AdFalcon.EventDTOs;
using ArabyAds.AdFalcon.Maintenance.PGWTracker.Utilities.PaymentGateways;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Account;
using ArabyAds.AdFalcon.Services.Interfaces.DTOs.Reports;
using ArabyAds.AdFalcon.Services.Interfaces.Messages;
using ArabyAds.AdFalcon.Services.Interfaces.Services;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Account.Fund;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Core;
using ArabyAds.AdFalcon.Services.Interfaces.Services.Reports;
using ArabyAds.Framework;
using ArabyAds.Framework.DistributedEventBroker.PubSub.Entities;
using ArabyAds.Framework.DistributedEventBroker.PubSub.Subscription;
using ArabyAds.Framework.ExceptionHandling;
using ArabyAds.Framework.Utilities;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace ArabyAds.AdFalcon.AdFalconPortalMaintenanceJob.Core
{
    public class AdFalconPortalMaintenanceJob
    {
        private EventSubscriptionProxy KafkaProxy;
        private Timer _InitializeKafkaEventTimer;
        private Timer _ServiceTimer;
        private Timer _JobTimer;
        private Timer _IntializeJobTimer;
        private Timer _DeleteFilesTimer;
        private Timer _DeleteAuditTrialTimer;
        private Timer _warmUpTimer;
        private Timer _VideoConversionTimer;
        private Timer _ImpressionLogsTimer;
        private IFundTransactionService _FundTransactionService;
        private IReportService _ReportService;
        private IPaymentGatewayHelperFactory _PaymentHelperFactory;
        private IAccountService _AccountService;
        private IAdvertiserService _AdvertiserService;
        private IAudienceSegmentService _AudienceSegmentService;
        private IDeviceService _DeviceService;
        private IKeywordService _KeywordService;
        private ILocationService _LocationService;
        private IOperatorService _OperatorService;
        private IPartyService _PartyService;
        private ICreativeUnitService _CreativeUnitService;
        private Timer _SegmentsTimer;

      
        public AdFalconPortalMaintenanceJob()
        {
            _ServiceTimer = new Timer();
            _IntializeJobTimer = new Timer();
            _JobTimer = new Timer();
            _DeleteFilesTimer = new Timer();
            _DeleteAuditTrialTimer = new Timer();
            _ImpressionLogsTimer = new Timer();
            _InitializeKafkaEventTimer = new Timer();
            _warmUpTimer = new Timer();
            _VideoConversionTimer = new Timer();
            _SegmentsTimer = new Timer();
        }

        internal void Start()
        {


            //exception handling
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Framework, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Domain, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.ServiceLayer, new ExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.UI, new LogExceptionHandler());
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.WinServices, new LogExceptionHandler(), false);
            ExceptionPolicy.RegisterHandler<Exception>(ExceptionHandlingPolicies.Threading, new ExceptionHandler(), false);

            ApplicationContext.Instance.Logger.Info("ArabyAds.AdFalconPortalMaintenanceJob has started.");
            Directory.CreateDirectory(JsonConfigurationManager.AppSettings["ReportFolderCreation"]);
            Directory.CreateDirectory(JsonConfigurationManager.AppSettings["VideoFolderCreation"]);


            //log4net.LogManager.GetLogger().Info("PGWTrackingService has started.");
            _ServiceTimer.Enabled = false;
            _ServiceTimer.Elapsed += _ServiceTimer_Elapsed;

            _JobTimer.Enabled = false;
            _JobTimer.Elapsed += _JobTimer_Elapsed;

            _DeleteFilesTimer.Elapsed += _DeleteFilesTimer_Elapsed;
            _DeleteFilesTimer.Enabled = true;


            _warmUpTimer.Enabled = false;
            _warmUpTimer.Elapsed += WarmUpMethod;


            _VideoConversionTimer.Enabled = false;
            _VideoConversionTimer.Elapsed += videoConversionMethod;


            _SegmentsTimer.Enabled = false;
            _SegmentsTimer.Elapsed += SegmentsMethod;

            _IntializeJobTimer.Enabled = false;
            _IntializeJobTimer.Elapsed += _InitializeJobTimer_Elapsed;
            _IntializeJobTimer.Enabled = true;



            _DeleteAuditTrialTimer.Enabled = false;
            _DeleteAuditTrialTimer.Elapsed += _DeleteAuditTrialTimer_Elapsed;

            if (!string.IsNullOrEmpty(JsonConfigurationManager.AppSettings["DeleteAuditTrialInterval"]))
                _DeleteAuditTrialTimer.Interval = Convert.ToInt64(JsonConfigurationManager.AppSettings["DeleteAuditTrialInterval"]);
            else
                _DeleteAuditTrialTimer.Interval = 86400000;


            _InitializeKafkaEventTimer.Enabled = false;
            _InitializeKafkaEventTimer.Elapsed += InitilizeKafkaEventsTimer_Elapsed;
            _InitializeKafkaEventTimer.Enabled = true;

            //_IntializeJobTimer.Enabled = false;
            //_IntializeJobTimer.Elapsed += _InitializeJobTimer_Elapsed;
            //_IntializeJobTimer.Enabled = true;
            _ImpressionLogsTimer.Enabled = false;
            _ImpressionLogsTimer.Elapsed += _ImpressionLogsTimer_Elapsed;
            //_ImpressionLogsTimer.Enabled = true;


        }
        void InitilizeKafkaEventsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {


                _InitializeKafkaEventTimer.Enabled = false;

                //   Framework.EventBroker.EventBroker.Instance.Raise(new Framework.EventBroker.EventArgsBase("CampaignPaused", 103626.ToString(), null,null));

                // Framework.EventBroker.EventBroker.Instance.Flush();
                var config = new SubscriptionProxyConfig { LoggerName = "event.pubsub", ProxyId = JsonConfigurationManager.AppSettings["KafkaSubHostName"], OffsetTrackingMode = OffsetTrackingMode.Strict, ZooKeeperTimeout = TimeSpan.Parse(JsonConfigurationManager.AppSettings["event.pubsub.zkSessionTimeOut"]), ZooKeeperUrl = JsonConfigurationManager.AppSettings["event.pubsub.zkServiceUrl"], ZooKeeperConnectionStr = JsonConfigurationManager.AppSettings["event.pubsub.zkConnectionString"] };
                KafkaProxy = EventSubscriptionProxy.Create(config);
                KafkaProxy.Init();
                Task.Delay(1000);
                KafkaProxy.SubscribeAsync<CampaignOverspend>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignInvoiced).Wait();
                KafkaProxy.SubscribeAsync<CampaignBillingInfoChangedAck>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignBillingInfoChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<AdGroupBillingInfoChangedAck>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.AdGroupBillingInfoChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<FundOverSpend>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.FundOverSpendProcessed).Wait();

                KafkaProxy.SubscribeAsync<CampaignFundAboutTobeConsumed>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignFundAboutTobeConsumedProcessed).Wait();
                KafkaProxy.SubscribeAsync<CampaignStatusChanged>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignStatusChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<AudienceListCheck>(JsonConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.AudienceListProcessed).Wait();


            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

                _InitializeKafkaEventTimer.Enabled = true;
            }

        }


        void SegmentsMethod(object sender, ElapsedEventArgs e)
        {

            try
            {
                _SegmentsTimer.Enabled = false;


                Segments segHelper = new Segments();

                segHelper.ResolveSegmentsFiles();
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
            }
            finally
            {
                _SegmentsTimer.Interval = int.Parse(JsonConfigurationManager.AppSettings["TimerMinutes"]) * 30 * 600;
                _SegmentsTimer.Enabled = true;
            }


        }
        void videoConversionMethod(object sender, ElapsedEventArgs e)
        {

            try
            {
                _VideoConversionTimer.Enabled = false;


                VideoConversionHelper videoConversion = new VideoConversionHelper();

                videoConversion.ResolveDraftVideos();
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
            }
            finally
            {
                _VideoConversionTimer.Interval = int.Parse(JsonConfigurationManager.AppSettings["TimerMinutes"]) * 30 * 600;
                _VideoConversionTimer.Enabled = true;
            }


        }
        void WarmUpMethod(object sender, ElapsedEventArgs e)
        {

            ApplicationContext.Instance.Logger.Info("Start Warm Up");
            _warmUpTimer.Enabled = false;


            try
            {

                _AdvertiserService.GetAll();

                _DeviceService.GetAll();
                _DeviceService.GetAllDeviceTree();

                _KeywordService.GetAll();
                _LocationService.GetAll();
                _CreativeUnitService.GetAll();
                _CreativeUnitService.GetAllSupportedFormat();
                _AudienceSegmentService.GetAll(null);
                _OperatorService.GetAll();
                _OperatorService.GetAllCountryOperator();
            }
            catch (Exception ex)
            {

                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
            }
            finally
            {
                _warmUpTimer.Interval = 3600000;
                _warmUpTimer.Enabled = true;


            }
            ApplicationContext.Instance.Logger.Info("Finish Warm Up");

        }
        void _DeleteAuditTrialTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _DeleteAuditTrialTimer.Enabled = false;
            bool active = Convert.ToBoolean(JsonConfigurationManager.AppSettings["DeleteAuditTrialActive"]);
            if (active)
            {
                try
                {
                    Framework.ApplicationContext.Instance.Logger.Warn("Delete AuditTrail started");


                    int years = Convert.ToInt32(JsonConfigurationManager.AppSettings["DeleteAuditTrialYears"]);

                    //Stopwatch stopWatch = new Stopwatch();
                    //stopWatch.Start();
                    DeleteAuditTrial(years);
                    DeleteAuditTrialSessionStat(years);
                    DeleteAuditTrialStat(years);
                    // stopWatch.Stop();

                    //Framework.ApplicationContext.Instance.Logger.Warn("toke : "+TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds).TotalSeconds.ToString());

                    Framework.ApplicationContext.Instance.Logger.Warn("Delete AuditTrail  ended");

                }
                catch (Exception ex)
                {
                    Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                }
                finally
                {

                    _DeleteAuditTrialTimer.Enabled = true;
                }
            }

        }
        void _InitializeJobTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool NotcatchHappen = false;

            try
            {
                _IntializeJobTimer.Enabled = false;
                _IntializeJobTimer.Interval = 10 * 1000;
                //this redo of work in 
                //SchedulerHelper.ReportService = IoC.Instance.Resolve<IReportService>();
                //SchedulerHelper.MailSender = IoC.Instance.Resolve<Framework.Utilities.EmailsSender.IMailSender>();
                //SchedulerHelper.ResourceService = IoC.Instance.Resolve<ArabyAds.Framework.Resources.IResourceService>();
                //SchedulerHelper.ConfigurationManager = Framework.IoC.Instance.Resolve<ArabyAds.Framework.ConfigurationSetting.IConfigurationSettingService>();
                //SchedulerHelper.SchedulerFactory = new StdSchedulerFactory();
                //SchedulerHelper.DefaultLanguage = JsonConfigurationManager.AppSettings["DefaultLanguage"];
                //SchedulerHelper.Scheduler = SchedulerHelper.SchedulerFactory.GetScheduler();
                //to make sure always initialize bcz eror of starting
                if (SchedulerHelper.Initialized == false)
                    SchedulerHelper.InitializeSchedulerHelper();
                _ReportService = SchedulerHelper.ReportService;
                _FundTransactionService = IoC.Instance.Resolve<IFundTransactionService>();
                _AccountService = IoC.Instance.Resolve<IAccountService>();
                _PaymentHelperFactory = new PaymentGatewayHelperFactory();
                _AdvertiserService = IoC.Instance.Resolve<IAdvertiserService>();
                _DeviceService = IoC.Instance.Resolve<IDeviceService>();
                _KeywordService = IoC.Instance.Resolve<IKeywordService>();

                _CreativeUnitService = IoC.Instance.Resolve<ICreativeUnitService>();
                _AudienceSegmentService = IoC.Instance.Resolve<IAudienceSegmentService>();
                _LocationService = IoC.Instance.Resolve<ILocationService>();
                _OperatorService = IoC.Instance.Resolve<IOperatorService>();
                _PartyService = IoC.Instance.Resolve<IPartyService>();
                SchedulerHelper.Scheduler.ResumeAll().Wait();
                SchedulerHelper.Scheduler.Start().Wait();
                NotcatchHappen = true;

            }
            catch (Exception ex)
            {


                _IntializeJobTimer.Enabled = true;
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);

            }
            finally
            {

            }
            if (NotcatchHappen)
            {
                _IntializeJobTimer.Enabled = false;

                _ServiceTimer.Enabled = true;
                _JobTimer.Enabled = true;

                _DeleteAuditTrialTimer.Enabled = true;
                var EnableWarpUpKey = JsonConfigurationManager.AppSettings["EnableWarpUp"];

                if (!string.IsNullOrEmpty(EnableWarpUpKey) && EnableWarpUpKey.ToLower() == "true")
                    _warmUpTimer.Enabled = true;
                _SegmentsTimer.Enabled = true;
                _VideoConversionTimer.Enabled = true;
                _ImpressionLogsTimer.Enabled = true;
            }

        }
        void _ServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _ServiceTimer.Enabled = false;
                _ServiceTimer.Interval = int.Parse(JsonConfigurationManager.AppSettings["TimerMinutes"]) * 60 * 1000;


                ICollection<PgwDto> paymentGateWays = _FundTransactionService.GetRegistredPGWs();

                // Iterate over registered payment gateways and resolve pending transactions for each one
                foreach (var gateway in paymentGateWays)
                {
                    IPaymentGatewayHelper helper = _PaymentHelperFactory.CreatePaymentHelper(gateway.Code);

                    //Resolve pending transactions 
                    helper.ResolvePendingTransactions();
                }
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
            }
            finally
            {

                _ServiceTimer.Enabled = true;
            }
        }

        void _DeleteFilesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _DeleteFilesTimer.Enabled = false;

            try
            {
                DateTime dtTime;
                string[] filePaths = Directory.GetFiles(JsonConfigurationManager.AppSettings["ReportFolderCreation"]);
                foreach (string filePath in filePaths)
                {
                    dtTime = File.GetCreationTime(filePath);
                    if (dtTime < DateTime.Now.AddHours(-1))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                //to make sure always up and running always
                if(!SchedulerHelper.Scheduler.IsStarted)
                   SchedulerHelper.Scheduler.Start().Wait();
            }
            finally
            {
                _DeleteFilesTimer.Interval = int.Parse(JsonConfigurationManager.AppSettings["TimerMinutes"]) * 500000;
                _DeleteFilesTimer.Enabled = true;

            }
        }
        void _ImpressionLogsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ImpressionLogsTimer.Enabled = false;
            var dt = Framework.Utilities.Environment.GetServerTime();

            try
            {
                if (dt.Hour >= 3 && dt.Hour <= Convert.ToInt32(JsonConfigurationManager.AppSettings["ImpressionLogsTimerHourScale"]))
                    ProcessFTPForDataProviders();

            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                //to make sure always up and running always

            }
            finally
            {
                _ImpressionLogsTimer.Interval = 60 * 1 * 60 * 1000;
                _ImpressionLogsTimer.Enabled = true;

            }

        }
        void _JobTimer_Elapsed(object sender, ElapsedEventArgs e)
        {


            _JobTimer.Enabled = false;
            int tempID = 0;

            try

            {
                HashSet<ITrigger> set;

                List<ReportSchedulerDto> dtos1 = _ReportService.GetCampaignJobsToSchduled();
                List<ReportSchedulerDto> dtos2 = _ReportService.GetAppsJobsToSchduled();
                dtos1 = dtos1.Concat(dtos2).ToList();
                string groupNamePublisher = "Publisher";
                string groupNameAdvertiser = "Advertiser";
                string groupName = groupNamePublisher;
                ITrigger trigger = null;
                ITrigger trigger2 = null;
                ITrigger trigger3 = null;
                int monthDay = 0;
                string triggerGroupName = string.Empty;
                string triggerName = string.Empty;
                DateTime TimeSentAt = Framework.Utilities.Environment.GetServerTime();

                // Iterate over registered payment gateways and resolve pending transactions for each one
                foreach (var item in dtos1)
                {
                    tempID = item.ID;
                    TimeSentAt = item.TimeSentAt.HasValue ? item.TimeSentAt.Value : Framework.Utilities.Environment.GetServerTime();
                    monthDay = 0;
                    if (item.ReportSectionType == ReportSectionType.Advertiser)
                    {
                        groupName = groupNameAdvertiser;
                    }
                    else
                    {

                        groupName = groupNamePublisher;
                    }


                    if (item.IsSendNow)
                    {
                        var jobNow = JobBuilder.Create<GenerateReport>()
 .WithIdentity(groupName + item.ID + "JobNow", groupName)
 .UsingJobData("ID", item.ID)

 .StoreDurably()
 .Build();

                        triggerGroupName = groupName + "SendNow";
                        triggerName = groupName + item.ID + "JobNow";

                        var datetoSTart = DateTime.Now;

                        var datetoSend = DateTime.Now.AddMinutes(1);
                        var dateToEnd = datetoSend.AddMinutes(15).ToUniversalTime();
                        trigger = TriggerBuilder.Create()

                                                                .WithIdentity(triggerName, triggerGroupName)
                                                                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(datetoSend.Hour, datetoSend.Minute))

                                                                .EndAt(dateToEnd)

                                                                .StartAt(datetoSTart.ToUniversalTime())
                                                                .ForJob(jobNow)
                                                                .Build();
                        set = new HashSet<ITrigger>() { trigger };
                        SchedulerHelper.Scheduler.ScheduleJob(jobNow, set, true).Wait();
                        _ReportService.UpdateJobToSendNow(ValueMessageWrapper.Create(item.ID));
                        continue;
                    }

                    if (item.EndDate.HasValue)
                    {
                        if (item.EndDate < Framework.Utilities.Environment.GetServerTime())
                        {
                            continue;
                        }

                    }

                    var job = JobBuilder.Create<GenerateReport>()
        .WithIdentity(groupName + item.ID, groupName)
        .UsingJobData("ID", item.ID)

        .StoreDurably()
        .Build();
                    if (item.RecurrenceType == RecurrenceType.Day)
                    {
                        triggerGroupName = groupName + "Day";
                        triggerName = groupName + item.ID;
                        trigger = TriggerBuilder.Create()

                                                                .WithIdentity(triggerName, triggerGroupName)
                                                                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(TimeSentAt.Hour, TimeSentAt.Minute).WithMisfireHandlingInstructionDoNothing())
                                                                .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)
                                                                .StartAt(item.StartDate.ToUniversalTime())
                                                                .ForJob(job)

                                                                .Build();
                    }
                    if (item.RecurrenceType == RecurrenceType.Week)
                    {

                        int weekday = (int)item.WeekDay;
                        triggerGroupName = groupName + "Week";
                        triggerName = groupName + item.ID;


                        if (!string.IsNullOrEmpty(item.DaysOfWeekParams))
                        {
                            var list = GetDaysOfWeek(item);
                            if (!(list != null & list.Length > 0))
                            {
                                if (item.TriggerName != string.Empty && item.TriggerName != null)
                                {
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName))).Wait();

                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName))).Wait();
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName))).Wait();
                                }
                                else
                                {
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName))).Wait();
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName))).Wait();
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName))).Wait();
                                }
                                continue;
                            }
                            trigger = TriggerBuilder.Create()

                                                            .WithIdentity(triggerName, triggerGroupName)
                                                            .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(TimeSentAt.Hour, TimeSentAt.Minute, list).WithMisfireHandlingInstructionDoNothing())
                                                            .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)

                                                            .StartAt(item.StartDate.ToUniversalTime())
                                                            .ForJob(job)
                                                            .Build();
                        }
                        else
                        {
                            if (item.TriggerName != string.Empty && item.TriggerName != null)
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName))).Wait();

                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName))).Wait();
                            }
                            else
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName))).Wait();
                            }
                            continue;

                            //trigger = TriggerBuilder.Create()

                            //                             .WithIdentity(triggerName, triggerGroupName)
                            //                             .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Sunday,  TimeSentAt.Hour, TimeSentAt.Minute).WithMisfireHandlingInstructionDoNothing())
                            //                             .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)

                            //                             .StartAt(item.StartDate.ToUniversalTime())
                            //                             .ForJob(job)
                            //                             .Build();
                        }

                    }
                    if (item.RecurrenceType == RecurrenceType.Month)
                    {
                        triggerGroupName = groupName + "Month";
                        triggerName = groupName + item.ID;
                        monthDay = (int)item.MonthDay;

                        if (monthDay==0)
                        {
                            monthDay = 1;
                        }
                        trigger = TriggerBuilder.Create()

                                                                .WithIdentity(triggerName, triggerGroupName)
                                                                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(monthDay, TimeSentAt.Hour, TimeSentAt.Minute).WithMisfireHandlingInstructionDoNothing())
                                                                .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)
                                                                .StartAt(item.StartDate.ToUniversalTime())
                                                                .ForJob(job)
                                                                .Build();
                        if (monthDay > 28)
                        {
                            trigger2 = TriggerBuilder.Create()

                                                                    .WithIdentity(triggerName + 28 + "ForMonth", triggerGroupName)
                                                                    .WithSchedule(CronScheduleBuilder.CronSchedule(TimeSentAt.Hour + " " + TimeSentAt.Minute + " 0 28 2 ? *").WithMisfireHandlingInstructionDoNothing())
                                                                    .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)
                                                                    .StartAt(item.StartDate.ToUniversalTime())
                                                                    .ForJob(job)
                                                                    .Build();
                        }

                        if (monthDay == 31)
                        {
                            trigger3 = TriggerBuilder.Create()

                                                                    .WithIdentity(triggerName + 30 + "ForMonth", triggerGroupName)
                                                                    .WithSchedule(CronScheduleBuilder.CronSchedule(TimeSentAt.Hour + " " + TimeSentAt.Minute + " 0 30 4,6,9,11 ? *").WithMisfireHandlingInstructionDoNothing())
                                                                    .EndAt(item.EndDate.HasValue == true ? item.EndDate.Value.ToUniversalTime() : item.EndDate)
                                                                    .StartAt(item.StartDate.ToUniversalTime())
                                                                    .ForJob(job)
                                                                    .Build();
                        }



                    }


                    if (monthDay > 28)
                    {
                        set = new HashSet<ITrigger>() { trigger, trigger2 };
                        if (monthDay == 31)
                        {
                            set = new HashSet<ITrigger>() { trigger, trigger2, trigger3 };
                        }
                    }
                    else
                    {
                        set = new HashSet<ITrigger>() { trigger };

                    }

                    if (item.IsActive)
                    {
                        try

                        {

                            if (item.TriggerName != string.Empty && item.TriggerName != null)
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName))).Wait();

                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName))).Wait();
                            }
                            else
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName))).Wait();
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName))).Wait();
                            }


                            SchedulerHelper.Scheduler.ScheduleJob(job, set, true).Wait();
                        }
                        catch (Exception ex)
                        {

                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message + "JobID:" + item.ID, ex);
                            if (ex.GetType() == typeof(Quartz.SchedulerException))
                            {

                                _ReportService.UpdateJobToSchduled(new UpdateJobToSchduledRequest { JobID = item.ID, JobName = groupName + item.ID, TriggerName = triggerName, TriggerGroupName = triggerGroupName });
                            }
                        }


                    }
                    else
                    {

                        SchedulerHelper.Scheduler.AddJob(job, true).Wait();
                        if (item.TriggerName != string.Empty && item.TriggerName != null)
                        {
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName))).Wait();

                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName))).Wait();
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName))).Wait();
                        }
                        else
                        {
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName))).Wait();
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName))).Wait();
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName))).Wait();
                        }

                    }
                    var nextFireTime = SchedulerHelper.GetNextFireTime(new JobKey(groupName + item.ID, groupName));
                    DateTime? dtSch = null;
                    if (nextFireTime.HasValue)
                    {
                        dtSch = nextFireTime.Value.DateTime;
                    }
                    _ReportService.UpdateJobToSchduled(new UpdateJobToSchduledRequest { JobID = item.ID, JobName = groupName + item.ID, TriggerName = triggerName, TriggerGroupName = triggerGroupName, NextFireTime = dtSch });

                }
            }
            catch (Exception ex)
            {

                Framework.ApplicationContext.Instance.Logger.Error(ex.Message + "JobID:" + tempID, ex);

            }
            finally
            {
                // if (_JobTimer.Interval==100)
                //{
                _JobTimer.Interval = int.Parse(JsonConfigurationManager.AppSettings["TimerMinutes"]) * 10 * 60;
                _JobTimer.Enabled = true;


                //}


            }


        }
        private DayOfWeek[] GetDaysOfWeek(ReportSchedulerDto dto)
        {
            List<int> Ids = new List<int>();
            DayOfWeek[] enumArray = null;
            int tempId = 0;

            if (!string.IsNullOrEmpty(dto.DaysOfWeekParams))
            {
                string DaysOfWeekParams = dto.DaysOfWeekParams.Trim(new char[] { ',' });
                var arrString = DaysOfWeekParams.Split(new char[] { ',' });
                foreach (var id in arrString)
                {
                    tempId = -1;

                    if (id != "," & !string.IsNullOrEmpty(id) & !string.IsNullOrWhiteSpace(id))
                    {
                        int.TryParse(id, out tempId);
                        if (tempId > -1)
                        {
                            Ids.Add(tempId);

                        }
                    }
                }
            }
            if (Ids.Count > 0)
            {

                enumArray = Ids.Cast<DayOfWeek>().ToArray();
            }

            return enumArray;
        }
        internal void Stop()
        {
            SchedulerHelper.Scheduler.PauseAll().Wait();
            SchedulerHelper.Scheduler.Shutdown(false).Wait();
            if (KafkaProxy != null)
                KafkaProxy.ShutDown().Wait();
            Framework.ApplicationContext.Instance.Logger.Info("ArabyAds.AdFalconPortalMaintenanceJob has stopped.");
        }

        #region delete AuditTrial
        private void DeleteAuditTrial(int years)
        {
            try
            {
                long midPoint;
                var response = _AccountService.GetAuditTrialsMaxAndMin(ValueMessageWrapper.Create(years));

                if (response.Max > 0 && response.Min > 0)
                {

                    midPoint = response.Min + ((response.Max - response.Min) / 2);
                    _AccountService.DeleteAuditTrials(new AuditTrialsMaxAndMinMessage { Min = response.Min, Max = midPoint });

                    if (response.Max > response.Min)
                    {
                        _AccountService.DeleteAuditTrials(new AuditTrialsMaxAndMinMessage { Min = ++midPoint, Max = response.Max });
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private void DeleteAuditTrialSessionStat(int years)
        {
            try
            {
                long midPoint;
                var response = _AccountService.GetAuditTrialSessionStatAliasMaxAndMin(ValueMessageWrapper.Create(years));
                if (response.Max > 0 && response.Min > 0)
                {

                    midPoint = response.Min + ((response.Max - response.Min) / 2);
                    _AccountService.DeleteAuditTrialSessionStat(new AuditTrialsMaxAndMinMessage { Min = response.Min, Max = midPoint });

                    if (response.Max > response.Min)
                    {
                        _AccountService.DeleteAuditTrialSessionStat(new AuditTrialsMaxAndMinMessage { Min = ++midPoint, Max = response.Max });
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private void DeleteAuditTrialStat(int years)
        {
            try
            {
                long midPoint;
                var response = _AccountService.GetAuditTrialStatAliasMaxAndMin(ValueMessageWrapper.Create(years));
                if (response.Max > 0)
                {
                    midPoint = response.Min + ((response.Max - response.Min) / 2);
                    _AccountService.DeleteAuditTrialStat(new AuditTrialsMaxAndMinMessage { Min = response.Min, Max = midPoint });

                    if (response.Max > response.Min)
                    {
                        _AccountService.DeleteAuditTrialStat(new AuditTrialsMaxAndMinMessage { Min = ++midPoint, Max = response.Max });
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        #endregion
        public void ProcessFTPForDataProviders()
        {
            Framework.ApplicationContext.Instance.Logger.Info("ProcessFTPForDataProviders.");
            var dataProviders = _PartyService.GetDPPartnersFTP();
            string BaseWeb1 = JsonConfigurationManager.AppSettings["Web01LogImpPhysicalPath"];
            string BaseWeb2 = JsonConfigurationManager.AppSettings["Web02LogImpPhysicalPath"];
            int threShold = Convert.ToInt32(JsonConfigurationManager.AppSettings["LogImpThreShold"]);

            foreach (var dataProv in dataProviders)
            {

                var logs = _PartyService.GetImpressionLogsNotWrritten(ValueMessageWrapper.Create(dataProv.ID.Value));
                var name = string.Empty;
                Framework.ApplicationContext.Instance.Logger.Info("ProcessFTPForDataProviders:" + dataProv.Name);
                if (logs != null)

                {
                    foreach (var log in logs)
                    {

                        try
                        {
                            name = "impression_log_" + log.Day.ToString("yyyy_MM_dd") + ".csv.gz";
                            if (log.Type == Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
                                name = "admarkup_log_" + log.Day.ToString("yyyy_MM_dd") + ".csv.gz";
                            DownloadImpLog(log.Path, BaseWeb1 + dataProv.FTPURL + @"\" + name);
                            DownloadImpLog(log.Path, BaseWeb2 + dataProv.FTPURL + @"\" + name);
                            _PartyService.SaveImpressionLogWrritten(ValueMessageWrapper.Create(log.ID));

                        }
                        catch (Exception ex)
                        {
                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                            //to make sure always up and running always

                        }

                    }
                }




                DeleteImpLog(BaseWeb1 + dataProv.FTPURL, threShold);
                DeleteImpLog(BaseWeb2 + dataProv.FTPURL, threShold);
            }


        }
       
        public void DownloadImpLog(string ImPath, string SavePath)
        {

            var hDFSUtil = new ArabyAds.AdFalcon.Domain.Utilities.WebHDFSUtil(JsonConfigurationManager.AppSettings["LogImpPath"], JsonConfigurationManager.AppSettings["LogImpPath2"], "", new NetworkCredential(JsonConfigurationManager.AppSettings["WebHDFSUserName"], JsonConfigurationManager.AppSettings["WebHDFSPassword"], JsonConfigurationManager.AppSettings["WebHDFSDomain"]));



            ImPath = ImPath.Replace(@"\", @"/");

            var pathimp = HttpUtility.UrlEncode(ImPath, Encoding.UTF8);


            bool IsPhysical = Convert.ToBoolean(JsonConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            Framework.ApplicationContext.Instance.Logger.Info("DownloadImpLog:" + ImPath);

            dowloadPath = JsonConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
            var dowloadPath2 = JsonConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";

            Byte[] contentarr = null;
            try
            {

                contentarr = hDFSUtil.ReadFileByResponse(pathimp);





            }
            finally
            {

            }
            BinaryReader sr = new BinaryReader(new MemoryStream(contentarr));

            try
            {
                long startBytes = 0;
                string _EncodedData = pathimp;


                using (FileStream fs = new FileStream(SavePath, FileMode.Create))
                {
                    using (BinaryWriter w = new BinaryWriter(fs))
                    {
                        int maxCount = (int)Math.Ceiling((contentarr.Length - startBytes + 0.0) / 1024);

                        for (var i = 0; i < maxCount; i++)
                        {
                            w.Write(sr.ReadBytes(1024));
                            w.Flush();
                        }
                    }
                }


            }
            finally
            {
                // Response.End();
                sr.Close();
            }



        }
        public void DeleteImpLog(string BasePath, int threShold)
        {
            Framework.ApplicationContext.Instance.Logger.Info("DownloadImpLog:" + BasePath);

            string[] files = Directory.GetFiles(BasePath);
            if (files != null)
            {
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < Framework.Utilities.Environment.GetServerTime().AddDays(-1 * (threShold + 1)))
                        fi.Delete();
                }
            }

        }
    }
}
