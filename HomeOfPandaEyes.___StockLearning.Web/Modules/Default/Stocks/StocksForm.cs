
namespace HomeOfPandaEyes.StockLearning.Default.Forms
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [FormScript("Default.Stocks")]
    [BasedOnRow(typeof(Entities.StocksRow), CheckNames = true)]
    public class StocksForm
    {
        public String StockName { get; set; }
    }
}