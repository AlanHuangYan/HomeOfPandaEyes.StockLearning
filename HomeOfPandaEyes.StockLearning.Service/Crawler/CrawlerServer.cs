using HomeOfPandaEyes.Infrastructure.Logger;
using HomeOfPandaEyes.StockLearning.Core;
using HomeOfPandaEyes.StockLearning.Core.Modules.Email;
using System;

namespace HomeOfPandaEyes.StockLearning.Service
{
    /// <summary>
    /// The main server logic.
    /// </summary>
    public class CrawlerServer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JobServer"/> class.
        /// </summary>
	    public CrawlerServer()
        {
            //logger = LogManager.GetLogger(GetType());
            Initialize();
        }

        /// <summary>
        /// Initializes the instance of the <see cref="JobServer"/> class.
        /// </summary>
        private void Initialize()
        {
            try
            {
                LoggingService.LoggerConfig = "CrawlerService";

            }
            catch (Exception e)
            {
                LoggingService.Error(null, e);
                // throw;
            }
        }


        /// <summary>
        /// Starts this instance, delegates to scheduler.
        /// </summary>
        public void Start()
        {
            try
            {
                this.StartCrawler();
            }
            catch (Exception ex)
            {
                LoggingService.Error(null, ex);
                //throw;
            }

            LoggingService.Info(null, "StartCrawler started successfully");
        }

        SpiderFactory _spiderFactory = new SpiderFactory();

        private void StartCrawler()
        {
            LoggingService.Info(null, "StartCrawler process start ...");

            //_notificationCenterProvider.AutoMaintenance();
            foreach (var item in SpiderFactory.StockSpiders)
            {
                item.Run();
            }

            LoggingService.Info(null, "StartCrawler process end.");

            // mail
            LoggingService.Info(null, "Send email process start ...");
            EmailService emailService = new EmailService();
            emailService.SendStock3521();
            LoggingService.Info(null, "Send email process end.");

            System.Threading.Thread.Sleep(10000);
        }
    }
}
