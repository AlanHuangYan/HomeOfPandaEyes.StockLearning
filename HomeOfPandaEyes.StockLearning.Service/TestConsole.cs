using HomeOfPandaEyes.Infrastructure.Logger;
using System;
using System.Linq;

namespace HomeOfPandaEyes.StockLearning.Service
{
    static class TestConsole
    {
        static void Main(string[] args)
        {
            try
            {
                var id = Guid.NewGuid();
                SetupConsoleTitle();

                //if (ConfirmServiceStartup("Starting StockLearning Service (y/n) ?"))
                //{
                //    string enable = ConfigurationManager.AppSettings["CrawlerServiceEnable"];
                //    if (!string.IsNullOrWhiteSpace(enable) &&
                //        enable.Trim().ToLower() == "true")
                //    {
                var service1 = new CrawlerService();
                        service1.Startup();

                        //var serviceAutoMaintenance = new NotificationSchedulingService.AutoMaintenanceService();
                        //serviceAutoMaintenance.Startup();
                //    }
                //}

            }
            catch (Exception ex)
            {
                LoggingService.Error(null, ex);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static bool ConfirmServiceStartup(string message)
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write(message);
                ConsoleKeyInfo keyinfo = Console.ReadKey();
                if (keyinfo.KeyChar == 'y' || keyinfo.KeyChar == 'Y')
                {
                    Console.WriteLine();
                    return true;
                }
                else if (keyinfo.KeyChar == 'n' || keyinfo.KeyChar == 'N')
                {
                    Console.WriteLine();
                    return false;
                }
            }
        }

        private static void SetupConsoleTitle()
        {
        }
    }
}
