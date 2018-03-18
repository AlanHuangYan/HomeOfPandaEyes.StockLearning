using Serenity.Services;

namespace HomeOfPandaEyes.StockLearning.Administration
{
    public class TranslationListRequest : ListRequest
    {
        public string SourceLanguageID { get; set; }
        public string TargetLanguageID { get; set; }
    }
}