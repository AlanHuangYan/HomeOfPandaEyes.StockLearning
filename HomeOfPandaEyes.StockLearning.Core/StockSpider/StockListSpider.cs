using DotnetSpider.Core;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using HomeOfPandaEyes.Infrastructure;
using HomeOfPandaEyes.StockLearning.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeOfPandaEyes.StockLearning.Core.StockSpider
{
    public class StockListSpider : IStockSpider
    {
        public void Run()
        {
            // Config encoding, header, cookie, proxy etc... 定义采集的 Site 对象, 设置 Header、Cookie、代理等
            //var site = new Site { EncodingName = "UTF-8" };
           var site = new Site();

            // Add start/feed urls. 添加初始采集链接
            site.AddStartUrl("http://quote.eastmoney.com/stocklist.html");

            DotnetSpider.Core.Spider spider = DotnetSpider.Core.Spider.Create(site,
                // use memoery queue scheduler. 使用内存调度
                new QueueDuplicateRemovedScheduler(),
                // use custmize processor for  Processor
                new StockListPageProcessor())
                // use custmize pipeline for  Pipeline
                .AddPipeline(new StockListPipeline());
            spider.Downloader = new HttpClientDownloader();
            spider.ThreadNum = 1;
            spider.EmptySleepTime = 3000;

            // Start crawler 启动爬虫
            spider.Run();
        }
    }

    internal class StockListPipeline : BasePipeline
    {
        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            var context = new StockLearningEntities();
            var stocks = context.Stocks.ToList();
            Encoding gb2312 = Encoding.GetEncoding("GB2312");

            foreach (var resultItem in resultItems)
            {
                foreach (var result in resultItem.Results)
                {
                    var resultValue = result.Value as List<string>;

                    foreach (var item in resultValue)
                    {
                        string stockValue = Utility.GetTitleContent(item, "a");
                        //var match = System.Text.RegularExpressions.Regex.Match(stockValue, @"(.*)\((.*)\)");
                        var match = System.Text.RegularExpressions.Regex.Match(item, @"<a target=""_blank"" href=""(?<href>.*)"">(?<name>.*)\((?<code>.*)\)</a>", RegexOptions.Singleline);
                        
                        if (match.Success)
                        {
                            string name = match.Groups["name"].Value;
                            string code = match.Groups["code"].Value;
                            string href = match.Groups["href"].Value;
                            string encoding = NPinyin.Pinyin.ConvertEncoding(name, Encoding.UTF8, gb2312);                            
                            string pinyin = NPinyin.Pinyin.GetInitials(encoding, gb2312);

                            if (code.StartsWith("6") || code.StartsWith("0") || code.StartsWith("3"))
                            {
                                var stock = stocks.FirstOrDefault(s => s.StockId == code);
                                if (stock != null)
                                {
                                    if (stock.StockName != name)
                                    {
                                        stock.StockName = name;
                                    }
                                    if (stock.Pinyin != pinyin)
                                    {
                                        stock.Pinyin = pinyin;
                                    }
                                    if (stock.WebAddress != href)
                                    {
                                        stock.WebAddress = href;
                                    }
                                }
                                else
                                {
                                    context.Stocks.Add(new Stock { StockId = code, StockName = name, Pinyin = pinyin, WebAddress = href });
                                }   
                            }
                        }
                    }
                    context.SaveChanges();
                }                 
            }
            // Other actions like save data to DB. 可以自由实现插入数据库或保存到文件
        }
    }

    internal class StockListPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            // 利用 Selectable 查询并构造自己想要的数据对象
            var totalElements = page.Selectable.SelectList(Selectors.Regex("<li><a target=\"_blank\" href=\"http://quote.eastmoney.com/.*?.html\">.*?</a></li>")).GetValues();

            List<string> results = new List<string>();
            foreach (var element in totalElements)
            {
                //string stockValue = Utility.GetTitleContent(element, "a");
                results.Add(element);
            }

            // Save data object by key. 以自定义KEY存入page对象中供Pipeline调用
            page.AddResultItem("Result", results);
        }
    }

    
}
