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
    /// <summary>
    /// 净利率,总资产周转率 爬虫,
    /// </summary>
    public class StockJJRSpider : IStockSpider
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
                string range = "sh";
                if (stock.StockId.StartsWith("0") || stock.StockId.StartsWith("3"))
                {
                    range = "sz";
                }
                site.AddStartUrl($"http://f9.eastmoney.com/{range}{stock.StockId}.html");
            }

            DotnetSpider.Core.Spider spider = DotnetSpider.Core.Spider.Create(site,
                // use memoery queue scheduler. 使用内存调度
                new QueueDuplicateRemovedScheduler(),
                // use custmize processor for  Processor
                new StockJJRPageProcessor())
                // use custmize pipeline for  Pipeline
                .AddPipeline(new StockJJRPipeline());
            spider.Downloader = new HttpClientDownloader();
            spider.ThreadNum = 4;
            spider.EmptySleepTime = 3000;

            // Start crawler 启动爬虫
            spider.Run();
        }
    }

    internal class StockJJRPipeline : BasePipeline
    {
        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            var context = new StockLearningEntities();
            string stockId = string.Empty;
            foreach (var resultItem in resultItems)
            {
                foreach (var result in resultItem.Results)
                {
                    if (result.Key == "StockID")
                    {
                        stockId = result.Value as string;
                        continue;
                    }

                    var resultValue = result.Value as DataTable;

                    if (resultValue.Rows.Count == 0)
                    {
                        return;
                    }

                    if (!resultValue.Columns.Contains("指标名称"))
                    {
                        continue;
                    }

                    Logger.Info($"Stock:{stockId}");

                    var reports = context.StockFinancialReports.Where(f => f.StockId == stockId).ToList();
                    List<object> reportDates = new List<object>();
                    List<object> jjls = new List<object>();

                    foreach (DataRow row in resultValue.Rows)
                    {

                        if (string.IsNullOrEmpty(row["指标名称"].ToString()))
                        {
                            continue;
                        }

                        decimal jlr = 0;
                        int unit = 0;
                        string strJLR = row["净利润(元)"].ToString();
                        if (strJLR.Contains("万"))
                        {
                            unit = 10000;
                        }
                        if (strJLR.Contains("亿"))
                        {
                            unit = 100000000;
                        }
                        strJLR = strJLR.Replace("万", string.Empty).Replace("亿", string.Empty);
                        decimal.TryParse(strJLR, out jlr);
                        jlr = jlr * unit;
                        
                        var reportDate = DateTime.ParseExact(row["指标名称"].ToString(),"yy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                        var report = reports.FirstOrDefault(r => r.ReportDate == reportDate);
                        decimal ZZCZZL = 0;
                        decimal.TryParse(row["总资产周转率(次)"].ToString(), out ZZCZZL);

                        if (report != null)
                        {
                            if (reportDate.AddYears(3) < DateTime.Now)
                            {
                                continue;
                            }
                            if (report.JLR== jlr && report.ZZCZZL == ZZCZZL)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        report.JLR = jlr;
                        report.YYJLL = report.YS > 0 ? report.JLR / report.YS * 100 : 0;
                        report.ZZCZZL = ZZCZZL;                        
                    }
                    context.SaveChanges();
                }                 
            }
            // Other actions like save data to DB. 可以自由实现插入数据库或保存到文件
        }
    }

    internal class StockJJRPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            // 利用 Selectable 查询并构造自己想要的数据对象
            int index = page.Content.IndexOf("重要指标");
            if (index < 0)
            {
                return;
            }
            string stockId = page.Url.Substring(26, 6); //http://f9.eastmoney.com/sh603999.html

            int indexTableStart = page.Content.IndexOf("<table", index);
            int indexTableEnd = page.Content.IndexOf("</table>", index);
            if (indexTableStart < 0 || indexTableEnd < 0)
            {
                return;
            }
            string strResult = page.Content.Substring(indexTableStart, indexTableEnd - indexTableStart + 8);

            var dataSet = Utility.ConvertHTMLTablesToDataSet(strResult);

            var dataTable = Utility.GenerateTransposedTable(dataSet.Tables[0]);

            //// Save data object by key. 以自定义KEY存入page对象中供Pipeline调用
            page.AddResultItem("StockID", stockId);
            page.AddResultItem("Result", dataTable);
        }
    }

    
}
