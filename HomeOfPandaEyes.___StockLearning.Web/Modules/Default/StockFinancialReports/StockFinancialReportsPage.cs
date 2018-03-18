
namespace HomeOfPandaEyes.StockLearning.Default.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Default/StockFinancialReports"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.StockFinancialReportsRow))]
    public class StockFinancialReportsController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Default/StockFinancialReports/StockFinancialReportsIndex.cshtml");
        }
    }
}