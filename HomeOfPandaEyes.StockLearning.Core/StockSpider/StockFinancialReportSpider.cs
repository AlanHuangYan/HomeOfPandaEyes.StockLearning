using DotnetSpider.Core;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using HomeOfPandaEyes.Infrastructure;
using HomeOfPandaEyes.StockLearning.DataContext;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeOfPandaEyes.StockLearning.Core.StockSpider
{
    public class StockFinancialReportSpider : IStockSpider
    {
        public void Run()
        {
            // Config encoding, header, cookie, proxy etc... 定义采集的 Site 对象, 设置 Header、Cookie、代理等
            //var site = new Site { EncodingName = "UTF-8" };
           var site = new Site();

            // Add start/feed urls. 添加初始采集链接
            var context = new StockLearningEntities();
            var stocks = context.Stocks.ToList();

            foreach (var stock in stocks)
            {
                site.AddStartUrl($"http://data.eastmoney.com/bbsj/stock{stock.StockId}/yjbb.html");
            }

            DotnetSpider.Core.Spider spider = DotnetSpider.Core.Spider.Create(site,
                // use memoery queue scheduler. 使用内存调度
                new QueueDuplicateRemovedScheduler(),
                // use custmize processor for  Processor
                new FinancialReportPageProcessor())
                // use custmize pipeline for  Pipeline
                .AddPipeline(new FinancialReportPipeline());
            spider.Downloader = new HttpClientDownloader();
            spider.ThreadNum = 4;
            spider.EmptySleepTime = 3000;

            // Start crawler 启动爬虫
            spider.Run();
        }
    }

    internal class FinancialReportPipeline : BasePipeline
    {
        private static long count = 0;

        //private readonly string SECUCODE = "SECUCODE";
        //private readonly string REPORTDATE = "REPORTDATE";
        //private readonly string YS = "YS";
        //private readonly string SJL = "SJL";
        //private readonly string EPSJB = "EPSJB";
        //private readonly string YSTZ = "YSTZ";
        //private readonly string YSHZ = "YSHZ";
        //private readonly string SJLTZ = "SJLTZ";
        //private readonly string SJLHZ = "SJLHZ";
        //private readonly string BPS = "BPS";
        //private readonly string ROEPJ = "ROEPJ";
        //private readonly string MGXJJE = "MGXJJE";
        //private readonly string XSMLL = "XSMLL";
        //private readonly string LRFP = "LRFP";
        //private readonly string GXL = "GXL";
        //private readonly string NoticeDate = "NoticeDate";

        private readonly string colSECUCODE = "scode";
        private readonly string colREPORTDATE = "REPORTDATE";
        private readonly string colYS = "totaloperatereve";
        private readonly string colSJL = "parentnetprofit";
        private readonly string colEPSJB = "basiceps";
        private readonly string colEPSKCJB = "cutbasiceps";        
        private readonly string colYSTZ = "YSTZ";
        private readonly string colYSHZ = "YSHZ";
        private readonly string colSJLTZ = "SJLTZ";
        private readonly string colSJLHZ = "SJLHZ";
        private readonly string colBPS = "BPS";
        private readonly string colROEPJ = "roeweighted";
        private readonly string colMGXJJE = "mgjyxjje";
        private readonly string colXSMLL = "XSMLL";
        private readonly string colLRFP = "assigndscrpt";
        private readonly string colGXL = "GXL";
        private readonly string colNoticeDate = "firstnoticedate";

        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            var context = new StockLearningEntities();            
            foreach (var resultItem in resultItems)
            {
                foreach (var result in resultItem.Results)
                { 
                    var resultValue = result.Value as DataTable;

                    if (resultValue.Rows.Count == 0)
                    {
                        return;
                    }
                    string stockId = resultValue.Rows[0][this.colSECUCODE].ToString();

                    Logger.Info($"Stock:{stockId}");

                    var reports = context.StockFinancialReports.Where(f => f.StockId == stockId).ToList();
                    foreach (DataRow row in resultValue.Rows)
                    {
                        var reportDate = Convert.ToDateTime(row[this.colREPORTDATE].ToString());

                        var report = reports.FirstOrDefault(r => r.ReportDate == reportDate);
                        if (report != null)
                        {
                            if (reportDate.AddYears(3) < DateTime.Now)
                            {
                                continue;
                            }
                            if (report.YS == Convert.ToDecimal(row[this.colYS]) && report.SJL == Convert.ToDecimal(row[this.colSJL]))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            report = new StockFinancialReport();
                            report.StockId = stockId;
                            report.ReportDate = reportDate;
                            context.StockFinancialReports.Add(report);
                            reports.Add(report);
                        }
                        report.EPSJB = Convert.ToDecimal(row[this.colEPSJB]);
                        decimal EPSKCJB = 0;
                        decimal.TryParse(row[this.colEPSKCJB].ToString(), out EPSKCJB);
                        report.EPSKCJB = EPSKCJB;
                        report.YS = Convert.ToDecimal(row[this.colYS]);
                        report.YSTZ = Convert.ToDecimal(row[this.colYSTZ]);
                        report.YSHZ = Convert.ToDecimal(row[this.colYSHZ]);
                        report.SJL = Convert.ToDecimal(row[this.colSJL]);
                        report.SJLTZ = Convert.ToDecimal(row[this.colSJLTZ]);
                        report.SJLHZ = Convert.ToDecimal(row[this.colSJLHZ]);
                        report.BPS = Convert.ToDecimal(row[this.colBPS]);
                        report.ROEPJ = Convert.ToDecimal(row[this.colROEPJ]);
                        decimal MGXJJE = 0;
                        decimal.TryParse(row[this.colMGXJJE].ToString(), out MGXJJE);
                        report.MGXJJE = MGXJJE;
                        report.XSMLL = Convert.ToDecimal(row[this.colXSMLL]);
                        report.LRFP = row[this.colLRFP].ToString();
                        report.GXL = Convert.ToDecimal(row[this.colGXL]);
                        report.NoticeDate = string.IsNullOrWhiteSpace(row[this.colNoticeDate].ToString()) ? new Nullable<DateTime>() : Convert.ToDateTime(row[this.colNoticeDate].ToString());
                        report.UpdatedDate = DateTime.Now;
                    }
                    context.SaveChanges();
                }                 
            }
            // Other actions like save data to DB. 可以自由实现插入数据库或保存到文件
        }
    }

    internal class FinancialReportPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            // 利用 Selectable 查询并构造自己想要的数据对象
            var element = page.Selectable.SelectList(Selectors.Regex(@"defjson: \{.*?\]\}")).GetValues().FirstOrDefault();

            if (string.IsNullOrEmpty(element))
            {
                return;
            }
            if (element.IndexOf("[{") < 0)
            {
                return;
            }
            element = element.Substring(element.IndexOf("[{"), element.Length - element.IndexOf("[{") - 1);
            element = element.Replace("\"NOTICEDATE\":\"-\"", "\"NOTICEDATE\":\"\"");
            element = element.Replace("\"-\"", "\"0\"");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
            var results = JsonConvert.DeserializeObject<DataTable>(element, settings);
                        

            //// Save data object by key. 以自定义KEY存入page对象中供Pipeline调用
            page.AddResultItem("Result", results);
        }
    }    
}
