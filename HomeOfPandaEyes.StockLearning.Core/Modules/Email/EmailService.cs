using HomeOfPandaEyes.Infrastructure;
using HomeOfPandaEyes.Infrastructure.Logger;
using HomeOfPandaEyes.StockLearning.DataContext;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeOfPandaEyes.StockLearning.Core.Modules.Email
{
    public class EmailService
    {
        public EmailService()
        {

        }

        public void SendStock3521()
        {
            StockLearning.DataContext.StockLearningEntities db = new StockLearningEntities();

            decimal YSTZ = 30; // 营业收入同比增长率
            decimal XSMLL = 50; // 销售毛利率
            decimal YYJLL = 20; // 净利率
            decimal ZZCZZL = 0.1M;
            var financialReports = (from p in db.StockFinancialReports.Include("Stock")
                                    where p.YSTZ >= YSTZ && p.XSMLL >= XSMLL && p.YYJLL >= YYJLL && p.UpdatedDate == System.DateTime.Today
                                    select p).ToList();


            if (financialReports.Any())
            {
                var mailSetting = db.SystemSettings.FirstOrDefault(s => s.SystemSettingKey == "MailSetting");

                if (mailSetting != null)
                {
                    string account = mailSetting.SystemSettingValue1;
                    string password = mailSetting.SystemSettingValue2;

                    string mailTemplate = GetEmbeddedResourceFile("HomeOfPandaEyes.StockLearning.Core.Modules.Email.Templates.Stock3521.html");

                    var users = db.Users.Where(u => u.IsActive == 1);
                    var config = new TemplateServiceConfiguration();
                    config.DisableTempFileLocking = true;
                    config.CachingProvider = new DefaultCachingProvider(t => { });
                    Engine.Razor = RazorEngineService.Create(config);
                    foreach (var user in users)
                    {
                        try
                        {
                            SMTPEmailSender sender = new SMTPEmailSender("smtp.gmail.com", account, password);
                            sender.AddReceiver(user.Email);
                            sender.Subject = "黑眼圈之家 - 3521选股";

                            string content = Engine.Razor.RunCompile(mailTemplate, "templateKey", null, new { UserName = user.DisplayName, Datas = financialReports });
                            sender.Content = content;
                            sender.Send();
                            
                        }
                        catch (System.Exception ex)
                        {
                            LoggingService.Error(null, ex);
                        }
                    }                    
                }
            }
        }

        // How to embedded a "Text file" inside of a C# project
        //   and read it as a resource from c# code:
        //
        // (1) Add Text File to Project.  example: 'myfile.txt'
        //
        // (2) Change Text File Properties:
        //      Build-action: EmbeddedResource
        //      Logical-name: myfile.txt      
        //          (note only 1 dot permitted in filename)
        //
        // (3) from c# get the string for the entire embedded file as follows:
        //
        //     string myfile = GetEmbeddedResourceFile("myfile.txt");

        public string GetEmbeddedResourceFile(string filename)
        {
            var ass = System.Reflection.Assembly.GetExecutingAssembly();
            using (var s = ass.GetManifestResourceStream(filename))
            using (var r = new System.IO.StreamReader(s, System.Text.Encoding.GetEncoding("gb2312")))
            {
                string result = r.ReadToEnd();
                return result;
            }
        }
    }
}
