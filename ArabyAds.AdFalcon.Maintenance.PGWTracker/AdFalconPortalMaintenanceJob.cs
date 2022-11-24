using Noqoush.Framework.ConfigurationSetting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Timers;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account.Fund;
using Noqoush.Framework;
using Noqoush.AdFalcon.Maintenance.PGWTracker.Utilities.PaymentGateways;

using Quartz;
using Quartz.Impl;
using Noqoush.AdFalcon.Services.Interfaces.Services.Reports;
using Noqoush.AdFalcon.Services.Interfaces.DTOs.Reports;
using Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.ReportSchedule;
using Noqoush.AdFalcon.Domain.Common.Model.Core;
using System.IO;
using Noqoush.AdFalcon.Domain.Common.Model.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Account;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign;
using Noqoush.AdFalcon.Services.Interfaces.Services.Core;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Interfaces.Services.Campaign.Creative;
using Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Video;
using Noqoush.Framework.DistributedEventBroker.PubSub.Subscription;
using Noqoush.Framework.DistributedEventBroker.PubSub.Entities;
using Noqoush.AdFalcon.EventDTOs;
using Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.Utilities.Events;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using GSSAPI;
using Noqoush.AdFalcon.AdFalconPortalMaintenanceJob.Utilities;
using Noqoush.AdFalcon.Domain.Utilities;

namespace Noqoush.AdFalcon.AdFalconPortalMaintenanceJob
{

    public partial class AdFalconPortalMaintenanceJob : ServiceBase
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

        //static void Main(string[] args)
        //{
        //    AdFalconPortalMaintenanceJob service = new AdFalconPortalMaintenanceJob();

        //    if (Environment.UserInteractive)
        //    {
        //        service.OnStart(args);
        //        Console.WriteLine("Press any key to stop program");
        //        Console.Read();
        //        service.OnStop();
        //    }
        //    else
        //    {
        //        ServiceBase.Run(service);
        //    }

        //}
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
            InitializeComponent();
            ServiceName = "Noqoush.AdFalconPortalMaintenanceJob";

        }

        protected override void OnStart(string[] args)
        {


     
            Framework.ApplicationContext.Instance.Logger.Info("Noqoush.AdFalconPortalMaintenanceJob has started.");
            Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"]);
            Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["VideoFolderCreation"]);

          
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

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DeleteAuditTrialInterval"]))
                _DeleteAuditTrialTimer.Interval = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["DeleteAuditTrialInterval"]);
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
                var config = new SubscriptionProxyConfig { LoggerName = "event.pubsub", ProxyId = System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"] , OffsetTrackingMode = OffsetTrackingMode.Strict, ZooKeeperTimeout= TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["event.pubsub.zkSessionTimeOut"]) , ZooKeeperUrl= System.Configuration.ConfigurationManager.AppSettings["event.pubsub.zkServiceUrl"], ZooKeeperConnectionStr = System.Configuration.ConfigurationManager.AppSettings["event.pubsub.zkConnectionString"] };
                 KafkaProxy = EventSubscriptionProxy.Create(config);
                KafkaProxy.Init();
                Task.Delay(1000);
                KafkaProxy.SubscribeAsync<CampaignOverspend>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignInvoiced).Wait();
                KafkaProxy.SubscribeAsync<CampaignBillingInfoChangedAck>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignBillingInfoChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<AdGroupBillingInfoChangedAck>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.AdGroupBillingInfoChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<FundOverSpend>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.FundOverSpendProcessed).Wait();

                KafkaProxy.SubscribeAsync<CampaignFundAboutTobeConsumed>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignFundAboutTobeConsumedProcessed).Wait();
                KafkaProxy.SubscribeAsync<CampaignStatusChanged>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.CampaignStatusChangedProcessed).Wait();
                KafkaProxy.SubscribeAsync<AudienceListCheck>(System.Configuration.ConfigurationManager.AppSettings["KafkaSubHostName"], KafkaEvents.AudienceListProcessed).Wait();

                
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
                _SegmentsTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerMinutes"]) * 13 * 60;
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
                _VideoConversionTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerMinutes"]) * 13 * 60;
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
            bool active = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["DeleteAuditTrialActive"]);
            if (active)
            {
                try
                {
                    Framework.ApplicationContext.Instance.Logger.Warn("Delete AuditTrail started");


                    int years = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DeleteAuditTrialYears"]);

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
                //SchedulerHelper.ResourceService = IoC.Instance.Resolve<Noqoush.Framework.Resources.IResourceService>();
                //SchedulerHelper.ConfigurationManager = Framework.IoC.Instance.Resolve<Noqoush.Framework.ConfigurationSetting.IConfigurationSettingService>();
                //SchedulerHelper.SchedulerFactory = new StdSchedulerFactory();
                //SchedulerHelper.DefaultLanguage = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"];
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
                SchedulerHelper.Scheduler.ResumeAll();
               SchedulerHelper.Scheduler.Start();
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
                var EnableWarpUpKey = System.Configuration.ConfigurationManager.AppSettings["EnableWarpUp"];

                if(!string.IsNullOrEmpty(EnableWarpUpKey) && EnableWarpUpKey.ToLower()=="true")
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
                _ServiceTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerMinutes"]) * 60 * 1000;


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
                string[] filePaths = Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["ReportFolderCreation"]);
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
                SchedulerHelper.Scheduler.Start();
            }
            finally
            {
                _DeleteFilesTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerMinutes"]) * 500000;
                _DeleteFilesTimer.Enabled = true;

            }
        }
        void _ImpressionLogsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ImpressionLogsTimer.Enabled = false;
            var dt= Framework.Utilities.Environment.GetServerTime();
       
            try
            {
                if(dt.Hour >=3 && dt.Hour <= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ImpressionLogsTimerHourScale"]))
                ProcessFTPForDataProviders();
                
            }
            catch (Exception ex)
            {
                Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                //to make sure always up and running always
                
            }
            finally
            {
                _ImpressionLogsTimer.Interval =  60*1*60* 1000;
                _ImpressionLogsTimer.Enabled = true;

            }

        }
        void _JobTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            //ReportCriteriaDto dsdsd = new ReportCriteriaDto();
            //dsdsd.MetricCode = "fdfdf";
            //dsdsd.TabId = "Opt";
            //dsdsd.FromDate = DateTime.Now;
            //dsdsd.ToDate = DateTime.Now.AddDays(-20);
            //dsdsd.CampaignType = CampaignType.Normal;
            //List<ReportRecipientDTO> AllReportRecipient = new List<ReportRecipientDTO>();
            //AllReportRecipient.Add(new ReportRecipientDTO { Email = "anashantash@yahoo.com" });
            //AllReportRecipient.Add(new ReportRecipientDTO { Email = "anasa@noqoush.com" });

            //ReportSchedulerDto testob = new ReportSchedulerDto
            //{
            //    Name = "fdfsdf4344",
            //    AllReportRecipient = AllReportRecipient,
            //    AccountId = 87062,
            //    TimeSentAt = DateTime.Now.AddMinutes(10),
            //    ReportDto = dsdsd,
            //    StartDate = DateTime.Now,
            //    RecurrenceType = RecurrenceType.Week,
            //    ReportSectionType = ReportSectionType.Publisher,
            //    WeekDay = WeekDay.Monday,
            //};
            //testReportSchedulerDto testob2 = new testReportSchedulerDto
            //{
            //    Name = "fdfsdf4344",
            //    AllReportRecipient = AllReportRecipient,
            //    AccountId = 87062,
            //    TimeSentAt = DateTime.Now.AddMinutes(10),

            //    StartDate = DateTime.Now,
            //    RecurrenceType = RecurrenceType.Week,
            //    ReportSectionType = ReportSectionType.Publisher,
            //    WeekDay = WeekDay.Monday,
            //};
            //_ReportService.SaveSchadulingReport(testob);
            //_ReportService.TestSaveSchadulingReport(testob2);
            //var results = _ReportService.GetSchadulingReport(80001);
            _JobTimer.Enabled = false;
            int tempID = 0;

            try

            {
                Quartz.Collection.HashSet<ITrigger> set;

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
                        set = new Quartz.Collection.HashSet<ITrigger>() { trigger };
                        SchedulerHelper.Scheduler.ScheduleJob(jobNow, set, true);
                        _ReportService.UpdateJobToSendNow(item.ID);
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
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName)));

                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName)));
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName)));
                                }
                                else
                                {
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName)));
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName)));
                                    SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName)));
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
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName)));

                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName)));
                            }
                            else
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName)));
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
                        set = new Quartz.Collection.HashSet<ITrigger>() { trigger, trigger2 };
                        if (monthDay == 31)
                        {
                            set = new Quartz.Collection.HashSet<ITrigger>() { trigger, trigger2, trigger3 };
                        }
                    }
                    else
                    {
                        set = new Quartz.Collection.HashSet<ITrigger>() { trigger };

                    }

                    if (item.IsActive)
                    {
                        try

                        {

                            if (item.TriggerName != string.Empty && item.TriggerName != null)
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName)));

                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName)));
                            }
                            else
                            {
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName)));
                                SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName)));
                            }


                            SchedulerHelper.Scheduler.ScheduleJob(job, set, true);
                        }
                        catch (Exception ex)
                        {

                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message + "JobID:" + item.ID, ex);
                            if (ex.GetType() == typeof(Quartz.SchedulerException))
                            {

                                _ReportService.UpdateJobToSchduled(item.ID, groupName + item.ID, triggerName, triggerGroupName, null);
                            }
                        }


                    }
                    else
                    {

                        SchedulerHelper.Scheduler.AddJob(job, true);
                        if (item.TriggerName != string.Empty && item.TriggerName != null)
                        {
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName, item.TriggerGroupName)));

                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 30 + "ForMonth", item.TriggerGroupName)));
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(item.TriggerName + 28 + "ForMonth", item.TriggerGroupName)));
                        }
                        else
                        {
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName, triggerGroupName)));
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 30 + "ForMonth", triggerGroupName)));
                            SchedulerHelper.Scheduler.UnscheduleJob((new TriggerKey(triggerName + 28 + "ForMonth", triggerGroupName)));
                        }

                    }
                    var nextFireTime = SchedulerHelper.GetNextFireTime(new JobKey(groupName + item.ID, groupName));
                    DateTime? dtSch = null;
                    if (nextFireTime.HasValue)
                    {
                        dtSch = nextFireTime.Value.DateTime;
                    }
                    _ReportService.UpdateJobToSchduled(item.ID, groupName + item.ID, triggerName, triggerGroupName, dtSch);

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
                _JobTimer.Interval = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimerMinutes"]) * 10 * 60;
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
        protected override void OnStop()
        {
            SchedulerHelper.Scheduler.PauseAll();
            SchedulerHelper.Scheduler.Shutdown(false);
            if(KafkaProxy!=null)
            KafkaProxy.ShutDown().Wait();
            Framework.ApplicationContext.Instance.Logger.Info("Noqoush.AdFalconPortalMaintenanceJob has stopped.");
        }

        #region delete AuditTrial
        private void DeleteAuditTrial(int years)
        {
            try
            {
                long max, min, midPoint;
                _AccountService.GetAuditTrialsMaxAndMin(years, out max, out min);

                if (max > 0 && min > 0)
                {

                    midPoint = min + ((max - min) / 2);
                    _AccountService.DeleteAuditTrials(min, midPoint);

                    if (max > min)
                    {
                        _AccountService.DeleteAuditTrials(++midPoint, max);
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
                long max, min, midPoint;
                _AccountService.GetAuditTrialSessionStatAliasMaxAndMin(years, out max, out min);
                if (max > 0 && min > 0)
                {

                    midPoint = min + ((max - min) / 2);
                    _AccountService.DeleteAuditTrialSessionStat(min, midPoint);

                    if (max > min)
                    {
                        _AccountService.DeleteAuditTrialSessionStat(++midPoint, max);
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
                long max, min, midPoint;
                _AccountService.GetAuditTrialStatAliasMaxAndMin(years, out max, out min);
                if (max > 0)
                {
                    midPoint = min + ((max - min) / 2);
                    _AccountService.DeleteAuditTrialStat(min, midPoint);

                    if (max > min)
                    {
                        _AccountService.DeleteAuditTrialStat(++midPoint, max);
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
            var dataProviders=  _PartyService.GetDPPartnersFTP();
          string BaseWeb1=  System.Configuration.ConfigurationManager.AppSettings["Web01LogImpPhysicalPath"];
            string BaseWeb2 = System.Configuration.ConfigurationManager.AppSettings["Web02LogImpPhysicalPath"];
            int threShold =Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["LogImpThreShold"]);

            foreach (var dataProv in dataProviders)
            {

                var logs = _PartyService.GetImpressionLogsNotWrritten(dataProv.ID.Value);
                var name = string.Empty;
                Framework.ApplicationContext.Instance.Logger.Info("ProcessFTPForDataProviders:"+dataProv.Name);
                if (logs!=null)

                {
                    foreach (var log in logs)
                    {
                     
                        try
                        {
                            name = "impression_log_" + log.Day.ToString("yyyy_MM_dd") + ".csv.gz";
                            if(log.Type==Domain.Common.Model.Account.DPP.ImpressionLogType.AdMarkup)
                            name = "admarkup_log_" + log.Day.ToString("yyyy_MM_dd") +".csv.gz";
                            DownloadImpLog(log.Path, BaseWeb1 + dataProv.FTPURL + @"\" + name);
                            DownloadImpLog(log.Path, BaseWeb2 + dataProv.FTPURL + @"\" + name);
                            _PartyService.SaveImpressionLogWrritten(log.ID);

                        }
                        catch (Exception ex)
                        {
                            Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                            //to make sure always up and running always

                        }
                       
                    }
                }

             
            

                DeleteImpLog(BaseWeb1+dataProv.FTPURL, threShold);
                DeleteImpLog(BaseWeb2 + dataProv.FTPURL, threShold);
            }
            

    }
        public  void DownloadImpLogOld( string ImPath, string SavePath)
        {

            ImPath = ImPath.Replace(@"\", @"/");

            var pathimp = HttpUtility.UrlEncode(ImPath, Encoding.UTF8);
            

            bool IsPhysical = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            Framework.ApplicationContext.Instance.Logger.Info("DownloadImpLog:" + ImPath);

            dowloadPath = System.Configuration.ConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
                var dowloadPath2 = System.Configuration.ConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";
                var cc = new CredentialCache();
                cc.Add(
                    new Uri(System.Configuration.ConfigurationManager.AppSettings["LogImpPath"]),
                    "NEGOTIATE",
                    new NetworkCredential("hdfs-reader-iadfalconcluster", "", ""));
                HttpWebResponse resp = null;
                HttpWebRequest req = null;
                try
                {
                    Gss.TerminateAndRemoveOverride();
                    Gss.InitializeAndOverrideApi();

                    req = (HttpWebRequest)WebRequest.Create(dowloadPath);
                    req.AllowAutoRedirect = true;
                    req.Credentials = cc;



                    try
                    {


                        resp = (HttpWebResponse)req.GetResponse();
                    }
                    catch (Exception ex)
                    {
                    Framework.ApplicationContext.Instance.Logger.Error(ex.Message, ex);
                    req = (HttpWebRequest)WebRequest.Create(dowloadPath2);
                        req.AllowAutoRedirect = true;
                        req.Credentials = cc;
                        resp = (HttpWebResponse)req.GetResponse();
                    }
                }
                finally
                {
                    Gss.TerminateAndRemoveOverride();

                }
                BinaryReader sr = new BinaryReader(resp.GetResponseStream());

                try
                {
                    long startBytes = 0;
                    string _EncodedData = pathimp;
            

                using (FileStream fs = new FileStream(SavePath, FileMode.Create))
                {
                    using (BinaryWriter w = new BinaryWriter(fs))
                    {
                        int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);

                       for (var  i = 0; i < maxCount  ; i++)
                           {
                            w.Write(sr.ReadBytes(1024));
                            w.Flush();
                        }
                    }
                }
                //  Response.Clear();
                //  Response.Buffer = false;
                //  Response.AddHeader("Accept-Ranges", "bytes");
                //  Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                // Response.ContentType = "application/octet-stream";
                // Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".csv.gz");
                // Response.AddHeader("Content-Length", (resp.ContentLength - startBytes).ToString());
                //Response.AddHeader("Connection", "Keep-Alive");
                // Response.ContentEncoding = Encoding.UTF8;

                //Dividing the data in 1024 bytes package
                //int maxCount = (int)Math.Ceiling((resp.ContentLength - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                //int i;
                //for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                //{
                //    Response.BinaryWrite(sr.ReadBytes(1024));
                //    Response.Flush();
                //}

            }
                finally
                {
                   // Response.End();
                    sr.Close();
                }
          


        }


        public void DownloadImpLog(string ImPath, string SavePath)
        {

            WebHDFSUtil hDFSUtil = new WebHDFSUtil(System.Configuration.ConfigurationManager.AppSettings["LogImpPath"], System.Configuration.ConfigurationManager.AppSettings["LogImpPath2"], "", new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["WebHDFSUserName"], System.Configuration.ConfigurationManager.AppSettings["WebHDFSPassword"], System.Configuration.ConfigurationManager.AppSettings["WebHDFSDomain"]));



            ImPath = ImPath.Replace(@"\", @"/");

            var pathimp = HttpUtility.UrlEncode(ImPath, Encoding.UTF8);


            bool IsPhysical = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LogImpIsPhysical"]);
            string dowloadPath = "";

            Framework.ApplicationContext.Instance.Logger.Info("DownloadImpLog:" + ImPath);

            dowloadPath = System.Configuration.ConfigurationManager.AppSettings["LogImpPath"] + pathimp + "?op=OPEN";
            var dowloadPath2 = System.Configuration.ConfigurationManager.AppSettings["LogImpPath2"] + pathimp + "?op=OPEN";
  
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
            if (files!=null)
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
