
namespace HomeOfPandaEyes.StockLearning.Default.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Default.Stocks")]
    [BasedOnRow(typeof(Entities.StocksRow), CheckNames = true)]
    public class StocksColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public String StockId { get; set; }
        public String StockName { get; set; }
    }
}