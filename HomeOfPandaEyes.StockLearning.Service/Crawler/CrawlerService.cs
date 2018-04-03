using HomeOfPandaEyes.Infrastructure.Logger;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Transactions;

namespace HomeOfPandaEyes.StockLearning.Service
{
    public partial class CrawlerService : ServiceBase, IServiceControl
    {

        private Timer _timer;
        private static Guid _serviceGuid;

        private const string CRAWLERSERVICE_PROCESS_MUTEX_PREFIX = "CrawlerServiceProcessMutex";
        private string scheduledProcessMutex;
        object locker = new object();
        private Mutex _mutext;
        private int scheduledIntervalSeconds;
        private TimeSpan scheduledProcessTime;

        public bool IsInitialized
        {
            get;
            private set;
        }
        public string ServiceModuleName { get { return "Crawler Service"; } }

        public CrawlerServer CrawlerServer { get; set; }


        #region Constructed
        public CrawlerService()
        {
            LoggingService.LoggerConfig = "CrawlerService";
            if (!int.TryParse(ConfigurationManager.AppSettings["CrawlerServiceIntervalSeconds"], out scheduledIntervalSeconds))
            {
                scheduledIntervalSeconds = 300000;
            }

            if (!TimeSpan.TryParseExact(ConfigurationManager.AppSettings["CrawlerServiceProcessTime"], "g", System.Globalization.CultureInfo.CurrentCulture, out scheduledProcessTime))
            {
                scheduledProcessTime = new TimeSpan(20, 0, 0);
            }

        }


        #endregion

        #region Service Handler
        protected override void OnStart(string[] args)
        {
            string startingMsg = string.Format("{0} {1}", ServiceModuleName, "Service is starting...");
            LoggingService.Info(null, startingMsg);

            InitializeService();

            string startedMsg = string.Format("{0} {1}", ServiceModuleName, "Service is started.");
            LoggingService.Info(null, startedMsg);
        }

        protected void InitializeService()
        {
            if (!IsInitialized)
            {
                try
                {
                    this.ResolveServices();
                    _timer = new Timer(ProcessSchedule, null, scheduledIntervalSeconds, Timeout.Infinite);
                    IsInitialized = true;
                }
                catch (Exception ex)
                {
                    LoggingService.Error(null, ex);
                }
            }
        }

        protected override void OnStop()
        {
            string stopingMsg = string.Format("{0} {1}", ServiceModuleName, "Service is stopping...");
            LoggingService.Info(null, stopingMsg);

            TerminateService();

            string stopedMsg = string.Format("{0} {1}", ServiceModuleName, "Service is stopped.");
            LoggingService.Info(null, stopedMsg);
        }

        public void Startup()
        {
            OnStart(null);
        }

        public void Endup()
        {
            OnStop();
        }
        #endregion

        /// <summary>
        /// resolve services here
        /// </summary>
        private void ResolveServices()
        {
        }

        /// <summary>
        /// Deal with all active alert batch process
        /// </summary>
        /// <param name="state"></param>
        private void ProcessSchedule(object state)
        {
            _mutext = null;
            _mutext = new Mutex(false, SCHEDULED_PROCESS_MUTEX);

            try
            {
                _mutext.WaitOne();
                LoggingService.LoggerConfig = "CrawlerService";

                string timerTriggerMsg = string.Format("{0} {1}", ServiceModuleName, "Timer tiggered !");
                LoggingService.Info(null, timerTriggerMsg);

                //if (DateTime.Now >= DateTime.Today.Add(scheduledProcessTime) && DateTime.Now < DateTime.Today.Add(scheduledProcessTime).AddMilliseconds(scheduledIntervalSeconds))
                //{
                    if (CrawlerServer == null)
                    {
                        CrawlerServer = new CrawlerServer();
                    }
                    CrawlerServer.Start();
                //}
                //else
                //{
                //    LoggingService.Info(null, "Time does not match.");
                //}
            }
            catch (TransactionAbortedException e)
            {
                LoggingService.Error(null, e);
            }
            catch (Exception ex)
            {
                LoggingService.Error(null, ex);
            }
            finally
            {
                _mutext.ReleaseMutex();
                GC.Collect();
                _timer.Change(scheduledIntervalSeconds, Timeout.Infinite);
            }
        }
        private string SCHEDULED_PROCESS_MUTEX
        {
            get
            {
                if (scheduledProcessMutex == null)
                {
                    scheduledProcessMutex = CRAWLERSERVICE_PROCESS_MUTEX_PREFIX + _serviceGuid.ToString();
                }
                return scheduledProcessMutex;
            }
        }

        private void TerminateService()
        {
            IsInitialized = false;
            _timer.Dispose();
        }
    }
}
