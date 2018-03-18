using HomeOfPandaEyes.StockLearning.Core.StockSpider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeOfPandaEyes.StockLearning.Core
{
    public class SpiderFactory
    {
        private static IList<IStockSpider> _stockSpiders;
        public static IList<IStockSpider> StockSpiders
        {
            get
            {
                if (_stockSpiders == null)
                {
                    Initialize();
                }
                return _stockSpiders;
            }
        }

        private static void Initialize()
        {
            _stockSpiders = new List<IStockSpider>
                              {
                                new StockSpider.StockListSpider(),
                                new StockSpider.StockFinancialReportSpider(),
                                new StockSpider.StockJJRSpider(),
                             };
        }
    }    
}
