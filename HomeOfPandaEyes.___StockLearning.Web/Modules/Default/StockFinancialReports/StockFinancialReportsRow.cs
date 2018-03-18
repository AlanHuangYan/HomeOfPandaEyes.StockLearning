
namespace HomeOfPandaEyes.StockLearning.Default.Entities
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey("Default"), Module("Default"), TableName("[dbo].[StockFinancialReports]")]
    [DisplayName("Stock Financial Reports"), InstanceName("Stock Financial Reports")]
    [ReadPermission("Administration:General")]
    [ModifyPermission("Administration:General")]
    public sealed class StockFinancialReportsRow : Row, IIdRow, INameRow
    {
        [DisplayName("Stock"), Size(10), PrimaryKey, ForeignKey("[dbo].[Stocks]", "StockId"), LeftJoin("jStock"), QuickSearch, TextualField("StockStockName")]
        public String StockId
        {
            get { return Fields.StockId[this]; }
            set { Fields.StockId[this] = value; }
        }

        [DisplayName("Report Date"), PrimaryKey]
        public DateTime? ReportDate
        {
            get { return Fields.ReportDate[this]; }
            set { Fields.ReportDate[this] = value; }
        }

        [DisplayName("Epsjb"), Column("EPSJB"), Size(19), Scale(4)]
        public Decimal? Epsjb
        {
            get { return Fields.Epsjb[this]; }
            set { Fields.Epsjb[this] = value; }
        }

        [DisplayName("Epskcjb"), Column("EPSKCJB"), Size(19), Scale(4)]
        public Decimal? Epskcjb
        {
            get { return Fields.Epskcjb[this]; }
            set { Fields.Epskcjb[this] = value; }
        }

        [DisplayName("Ys"), Column("YS"), Size(19), Scale(4)]
        public Decimal? Ys
        {
            get { return Fields.Ys[this]; }
            set { Fields.Ys[this] = value; }
        }

        [DisplayName("Ystz"), Column("YSTZ"), Size(18), Scale(8)]
        public Decimal? Ystz
        {
            get { return Fields.Ystz[this]; }
            set { Fields.Ystz[this] = value; }
        }

        [DisplayName("Yshz"), Column("YSHZ"), Size(18), Scale(8)]
        public Decimal? Yshz
        {
            get { return Fields.Yshz[this]; }
            set { Fields.Yshz[this] = value; }
        }

        [DisplayName("Sjl"), Column("SJL"), Size(19), Scale(4)]
        public Decimal? Sjl
        {
            get { return Fields.Sjl[this]; }
            set { Fields.Sjl[this] = value; }
        }

        [DisplayName("Sjltz"), Column("SJLTZ"), Size(18), Scale(8)]
        public Decimal? Sjltz
        {
            get { return Fields.Sjltz[this]; }
            set { Fields.Sjltz[this] = value; }
        }

        [DisplayName("Sjlhz"), Column("SJLHZ"), Size(18), Scale(8)]
        public Decimal? Sjlhz
        {
            get { return Fields.Sjlhz[this]; }
            set { Fields.Sjlhz[this] = value; }
        }

        [DisplayName("Bps"), Column("BPS"), Size(18), Scale(8)]
        public Decimal? Bps
        {
            get { return Fields.Bps[this]; }
            set { Fields.Bps[this] = value; }
        }

        [DisplayName("Roepj"), Column("ROEPJ"), Size(18), Scale(8)]
        public Decimal? Roepj
        {
            get { return Fields.Roepj[this]; }
            set { Fields.Roepj[this] = value; }
        }

        [DisplayName("Mgxjje"), Column("MGXJJE"), Size(18), Scale(8)]
        public Decimal? Mgxjje
        {
            get { return Fields.Mgxjje[this]; }
            set { Fields.Mgxjje[this] = value; }
        }

        [DisplayName("Xsmll"), Column("XSMLL"), Size(18), Scale(8)]
        public Decimal? Xsmll
        {
            get { return Fields.Xsmll[this]; }
            set { Fields.Xsmll[this] = value; }
        }

        [DisplayName("Lrfp"), Column("LRFP"), Size(50)]
        public String Lrfp
        {
            get { return Fields.Lrfp[this]; }
            set { Fields.Lrfp[this] = value; }
        }

        [DisplayName("Gxl"), Column("GXL"), Size(18), Scale(8)]
        public Decimal? Gxl
        {
            get { return Fields.Gxl[this]; }
            set { Fields.Gxl[this] = value; }
        }

        [DisplayName("Notice Date")]
        public DateTime? NoticeDate
        {
            get { return Fields.NoticeDate[this]; }
            set { Fields.NoticeDate[this] = value; }
        }

        [DisplayName("Updated Date")]
        public DateTime? UpdatedDate
        {
            get { return Fields.UpdatedDate[this]; }
            set { Fields.UpdatedDate[this] = value; }
        }

        [DisplayName("Stock Stock Name"), Expression("jStock.[StockName]")]
        public String StockStockName
        {
            get { return Fields.StockStockName[this]; }
            set { Fields.StockStockName[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.StockId; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.StockId; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public StockFinancialReportsRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public StringField StockId;
            public DateTimeField ReportDate;
            public DecimalField Epsjb;
            public DecimalField Epskcjb;
            public DecimalField Ys;
            public DecimalField Ystz;
            public DecimalField Yshz;
            public DecimalField Sjl;
            public DecimalField Sjltz;
            public DecimalField Sjlhz;
            public DecimalField Bps;
            public DecimalField Roepj;
            public DecimalField Mgxjje;
            public DecimalField Xsmll;
            public StringField Lrfp;
            public DecimalField Gxl;
            public DateTimeField NoticeDate;
            public DateTimeField UpdatedDate;

            public StringField StockStockName;
		}
    }
}
