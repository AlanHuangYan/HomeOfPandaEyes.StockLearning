
namespace HomeOfPandaEyes.StockLearning.Default.Entities
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey("Default"), Module("Default"), TableName("[dbo].[Stocks]")]
    [DisplayName("Stocks"), InstanceName("Stocks")]
    [ReadPermission("Administration:General")]
    [ModifyPermission("Administration:General")]
    public sealed class StocksRow : Row, IIdRow, INameRow
    {
        [DisplayName("Stock Id"), Size(10), PrimaryKey, QuickSearch]
        public String StockId
        {
            get { return Fields.StockId[this]; }
            set { Fields.StockId[this] = value; }
        }

        [DisplayName("Stock Name"), Size(50), NotNull]
        public String StockName
        {
            get { return Fields.StockName[this]; }
            set { Fields.StockName[this] = value; }
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

        public StocksRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public StringField StockId;
            public StringField StockName;
		}
    }
}
