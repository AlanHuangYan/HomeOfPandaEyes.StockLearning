using HomeOfPandaEyes.StockLearning.DataContext;
using HomeOfPandaEyes.StockLearning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeOfPandaEyes.StockLearning.Web.Controllers
{
    public class StockController : Controller
    {
        private StockLearning.DataContext.StockLearningEntities db = new StockLearningEntities();

        // GET: Stock
        public ActionResult Index()
        {
            HomeController.RootSearch(this);

            var data = new List<Stock>();
            if (HttpRuntime.Cache["Stocks"] != null)
            {
                data = HttpRuntime.Cache["Stocks"] as List<Stock>;
            }
            else
            {
                data = db.Stocks.ToList();
                HttpRuntime.Cache["Stocks"] = data;
            }
            return View(data);
        }

        public ActionResult Search(string id)
        {
            HomeController.RootSearch(this);

            var data = db.Stocks.Where(f => f.StockId.Contains(id) || f.Pinyin.Contains(id) || f.StockName.Contains(id)).ToList();
            return View("Index", data);
        }

        public ActionResult FinancialReport(string id)
        {
            HomeController.RootSearch(this);

            StockFinancialReportContext data = new StockFinancialReportContext();
            data.Stock = db.Stocks.FirstOrDefault(f => f.StockId == id);
            data.FinancialReports = db.StockFinancialReports.Where(f => f.StockId == id).OrderByDescending(f => f.ReportDate).ToList();
            return View(data);
        }

        public ActionResult LatestFinancialReport()
        {
            HomeController.RootSearch(this);

            StockFinancialReportContext data = new StockFinancialReportContext();
            var searchDate = DateTime.Today.AddDays(-7);
            var year = DateTime.Today.Year - 1;
            data.FinancialReports = db.StockFinancialReports.Include("Stock").Where(f => f.NoticeDate >= searchDate && f.ReportDate.Year >= year).OrderByDescending(f => f.NoticeDate).ToList();
            return View(data);
        }

        public ActionResult StockSelection3521()
        {
            HomeController.RootSearch(this);

            StockFinancialReportContext data = new StockFinancialReportContext();
            data.FinancialReports = new List<StockFinancialReport>();

            decimal YSTZ = 30; // 营业收入同比增长率
            decimal XSMLL = 50; // 销售毛利率
            decimal YYJLL = 20; // 净利率
            decimal count = 1; // 净资产收益率
            if (string.IsNullOrEmpty(Request.QueryString["YSTZ"]))
            {
                data.FinancialReports = (from p in db.StockFinancialReports.Include("Stock")
                                         join q in db.StockFinancialReports.GroupBy(f => f.StockId).Select(g => new { StockId = g.Key, ReportDate = g.Max(x => x.ReportDate) }) on new { StockId = p.StockId, ReportDate = p.ReportDate } equals new { StockId = q.StockId, ReportDate = q.ReportDate }
                                         where p.YSTZ >= YSTZ && p.XSMLL >= XSMLL && p.YYJLL >= YYJLL
                                         select p).ToList();

            }
            else
            {
                var reportDate = DateTime.Now.AddYears(-2).AddMonths(-3);
                if (decimal.TryParse(Request.QueryString["YSTZ"], out YSTZ) && decimal.TryParse(Request.QueryString["XSMLL"], out XSMLL)
                    && decimal.TryParse(Request.QueryString["ROEPJ"], out YYJLL) && decimal.TryParse(Request.QueryString["Count"], out count))
                {
                    var financialReports = db.StockFinancialReports.Include("Stock").Where(p => p.ReportDate > reportDate && p.YSTZ >= YSTZ && p.XSMLL >= XSMLL && p.YYJLL >= YYJLL).ToList();

                    var stockIds = financialReports.GroupBy(f => f.StockId).Select(g => new { StockId = g.Key, ReportCount = g.Count() }).Where(g => g.ReportCount >= count).Select(g => g.StockId).ToList();

                    data.FinancialReports = financialReports.Where(f => stockIds.Contains(f.StockId)).OrderBy(f=>f.StockId).ThenByDescending(f=>f.ReportDate).ToList();
                }
            }

            bool ZZCZZL;
            bool.TryParse(Request.QueryString["ZZCZZL"], out ZZCZZL);
            
            if (!ZZCZZL)
            {
                var stockIds1 = data.FinancialReports.Select(f => f.StockId).ToList();
                var reportDate1 = new DateTime(DateTime.Now.Year - 1, 12, 31);
                var reportDate2 = new DateTime(DateTime.Now.Year - 2, 12, 31);
                stockIds1 = db.StockFinancialReports.Where(f => stockIds1.Contains(f.StockId) && (f.ReportDate == reportDate1 || f.ReportDate == reportDate2) && f.ZZCZZL >= 1).Select(f => f.StockId).ToList();

                data.FinancialReports = data.FinancialReports.Where(f => stockIds1.Contains(f.StockId)).ToList();
            }
            
            return View(data);
        }
    }
}