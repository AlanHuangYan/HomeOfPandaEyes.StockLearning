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
                    string stockId = resultValue.Rows[0]["SECUCODE"].ToString();

                    Logger.Info($"Stock:{stockId}");

                    var reports = context.StockFinancialReports.Where(f => f.StockId == stockId).ToList();
                    foreach (DataRow row in resultValue.Rows)
                    {
                        var reportDate = Convert.ToDateTime(row["REPORTDATE"].ToString());

                        var report = reports.FirstOrDefault(r => r.ReportDate == reportDate);
                        if (report != null)
                        {
                            if (reportDate.AddYears(3) < DateTime.Now)
                            {
                                continue;
                            }
                            if (report.YS == Convert.ToDecimal(row["YS"]) && report.SJL == Convert.ToDecimal(row["SJL"]))
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
                        report.EPSJB = Convert.ToDecimal(row["EPSJB"]);
                        decimal EPSKCJB = 0;
                        decimal.TryParse(row["EPSKCJB"].ToString(), out EPSKCJB);
                        report.EPSKCJB = EPSKCJB;
                        report.YS = Convert.ToDecimal(row["YS"]);
                        report.YSTZ = Convert.ToDecimal(row["YSTZ"]);
                        report.YSHZ = Convert.ToDecimal(row["YSHZ"]);
                        report.SJL = Convert.ToDecimal(row["SJL"]);
                        report.SJLTZ = Convert.ToDecimal(row["SJLTZ"]);
                        report.SJLHZ = Convert.ToDecimal(row["SJLHZ"]);
                        report.BPS = Convert.ToDecimal(row["BPS"]);
                        report.ROEPJ = Convert.ToDecimal(row["ROEPJ"]);
                        decimal MGXJJE = 0;
                        decimal.TryParse(row["MGXJJE"].ToString(), out MGXJJE);
                        report.MGXJJE = MGXJJE;
                        report.XSMLL = Convert.ToDecimal(row["XSMLL"]);
                        report.LRFP = row["LRFP"].ToString();
                        report.GXL = Convert.ToDecimal(row["GXL"]);
                        report.NoticeDate = string.IsNullOrWhiteSpace(row["NoticeDate"].ToString()) ? new Nullable<DateTime>() : Convert.ToDateTime(row["NoticeDate"].ToString());
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
