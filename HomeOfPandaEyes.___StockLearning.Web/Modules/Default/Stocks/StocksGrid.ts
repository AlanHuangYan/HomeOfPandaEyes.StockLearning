
namespace HomeOfPandaEyes.StockLearning.Default {

    @Serenity.Decorators.registerClass()
    export class StocksGrid extends Serenity.EntityGrid<StocksRow, any> {
        protected getColumnsKey() { return 'Default.Stocks'; }
        protected getDialogType() { return StocksDialog; }
        protected getIdProperty() { return StocksRow.idProperty; }
        protected getLocalTextPrefix() { return StocksRow.localTextPrefix; }
        protected getService() { return StocksService.baseUrl; }

        constructor(container: JQuery) {
            super(container);
        }
        protected getColumns(): Slick.Column[] {
            var columns = super.getColumns();

            var fld = Default.StocksRow.Fields;

            Q.first(columns, x => x.field == fld.StockName).format =
                ctx => `<a href="javascript:;" class="customer-link">${Q.htmlEncode(ctx.value)}</a>`;

            
            return columns;
        }

        protected onClick(e: JQueryEventObject, row: number, cell: number) {
            super.onClick(e, row, cell);

            if (e.isDefaultPrevented())
                return;

            var item = this.itemAt(row);
            var target = $(e.target);

            var stockid = item.StockId;
            if (stockid.substring(0, 1) == '6')
            {
                stockid = 'sh' + stockid
            } else if (stockid.substring(0, 1) == '3') {
                stockid = 'sz' + stockid
            } else if (stockid.substring(0, 1) == '0') {
                stockid = 'sz' + stockid
            }

            if (target.hasClass('customer-link')) {
                e.preventDefault();

                window.open('http://quote.eastmoney.com/' + stockid + '.html', "_blank");
                //Q.postToUrl({
                //    url: 'http://quote.eastmoney.com/' + stockid + '.html',    
                //    params: {
                        
                //    },
                //    target: '_blank'
                //});
            }
        }
    }
}