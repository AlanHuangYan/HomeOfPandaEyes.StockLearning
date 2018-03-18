
namespace HomeOfPandaEyes.StockLearning.Administration
{
    using Serenity.Services;

    public class UserRoleListRequest : ServiceRequest
    {
        public int? UserID { get; set; }
    }
}