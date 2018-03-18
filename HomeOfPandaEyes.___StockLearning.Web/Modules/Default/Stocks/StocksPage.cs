
namespace HomeOfPandaEyes.StockLearning.Default.Pages
{
    using Serenity;
    using Serenity.Web;
    using System.Web.Mvc;

    [RoutePrefix("Default/Stocks"), Route("{action=index}")]
    [PageAuthorize(typeof(Entities.StocksRow))]
    public class StocksController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Modules/Default/Stocks/StocksIndex.cshtml");
        }
    }
}