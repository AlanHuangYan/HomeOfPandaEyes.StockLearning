//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeOfPandaEyes.StockLearning.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockFinancialReport
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public System.DateTime ReportDate { get; set; }
        public Nullable<decimal> EPSJB { get; set; }
        public Nullable<decimal> EPSKCJB { get; set; }
        public Nullable<decimal> YS { get; set; }
        public Nullable<decimal> YSTZ { get; set; }
        public Nullable<decimal> YSHZ { get; set; }
        public Nullable<decimal> SJL { get; set; }
        public Nullable<decimal> SJLTZ { get; set; }
        public Nullable<decimal> SJLHZ { get; set; }
        public Nullable<decimal> BPS { get; set; }
        public Nullable<decimal> ROEPJ { get; set; }
        public Nullable<decimal> MGXJJE { get; set; }
        public Nullable<decimal> XSMLL { get; set; }
        public string LRFP { get; set; }
        public Nullable<decimal> GXL { get; set; }
        public Nullable<System.DateTime> NoticeDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<decimal> JLR { get; set; }
        public Nullable<decimal> YYJLL { get; set; }
        public Nullable<decimal> ZZCZZL { get; set; }
    
        public virtual Stock Stock { get; set; }
    }
}