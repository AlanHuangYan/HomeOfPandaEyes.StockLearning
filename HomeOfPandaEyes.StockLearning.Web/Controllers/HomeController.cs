using HomeOfPandaEyes.StockLearning.Web.Models;
using System.Web.Mvc;
using System.Linq;

namespace HomeOfPandaEyes.StockLearning.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeController.RootSearch(this);

            StockSummaryContext data = new StockSummaryContext();

            using (var context = new DataContext.StockLearningEntities())
            {
                data.CountSH = context.Stocks.Count(s => s.StockId.StartsWith("6"));
                data.CountSZ = context.Stocks.Count(s => s.StockId.StartsWith("0"));
                data.CountCY = context.Stocks.Count(s => s.StockId.StartsWith("3"));
            }

            return View(data);
        }

        public static void RootSearch(Controller controller)
        {
            if (!string.IsNullOrEmpty(controller.Request.QueryString["q"]))
            {
                // Query string value is there so now use it
                controller.Response.Redirect($"~/stock/search/{controller.Request.QueryString["q"]}");
            }
        }
    }
}
