using HomeOfPandaEyes.StockLearning.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeOfPandaEyes.StockLearning.Web.Models
{
    public class StockFinancialReportContext
    {
        public Stock Stock { get; set; }

        public List<StockFinancialReport> FinancialReports { get; set; }
    }
}