using Serenity.Navigation;
using MyPages = HomeOfPandaEyes.StockLearning.Default.Pages;

[assembly: NavigationLink(int.MaxValue, "Stock/Stocks", typeof(MyPages.StocksController), icon: null)]
[assembly: NavigationLink(int.MaxValue, "Stock/Stock Financial Reports", typeof(MyPages.StockFinancialReportsController), icon: null)]