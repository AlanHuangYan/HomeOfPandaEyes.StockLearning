
namespace HomeOfPandaEyes.StockLearning.Default.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Default.StockFinancialReports")]
    [BasedOnRow(typeof(Entities.StockFinancialReportsRow), CheckNames = true)]
    public class StockFinancialReportsColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public String StockStockName { get; set; }
        public DateTime ReportDate { get; set; }
        public Decimal Epsjb { get; set; }
        public Decimal Epskcjb { get; set; }
        public Decimal Ys { get; set; }
        public Decimal Ystz { get; set; }
        public Decimal Yshz { get; set; }
        public Decimal Sjl { get; set; }
        public Decimal Sjltz { get; set; }
        public Decimal Sjlhz { get; set; }
        public Decimal Bps { get; set; }
        public Decimal Roepj { get; set; }
        public Decimal Mgxjje { get; set; }
        public Decimal Xsmll { get; set; }
        public String Lrfp { get; set; }
        public Decimal Gxl { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}