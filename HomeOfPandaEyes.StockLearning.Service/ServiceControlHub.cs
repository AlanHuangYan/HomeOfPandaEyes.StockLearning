using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceProcess;

namespace HomeOfPandaEyes.StockLearning.Service
{
    public partial class ServiceControlHub : ServiceBase
    {
        private readonly List<IServiceControl> _services = new List<IServiceControl>();

        public ServiceControlHub()
        {
            InitializeComponent();

            
            string enableService = ConfigurationManager.AppSettings["CrawlerServiceEnable"];
            if (!string.IsNullOrWhiteSpace(enableService) && enableService.Trim().ToLower() == "true")
            {
                _services.Add(new CrawlerService());
            }

        }

        protected override void OnStart(string[] args)
        {
            if (_services == null || !_services.Any())
            {
                return;
            }

            foreach (var service in _services.Where(service => service != null))
            {
                service.Startup();
            }
        }

        protected override void OnStop()
        {
            if (_services == null || !_services.Any()) return;
            foreach (var service in _services.Where(service => service != null))
            {
                service.Endup();
            }
        }
    }
}
