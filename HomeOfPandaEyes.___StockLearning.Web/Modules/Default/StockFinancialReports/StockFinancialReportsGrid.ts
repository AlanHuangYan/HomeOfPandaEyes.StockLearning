
namespace HomeOfPandaEyes.StockLearning.Default {

    @Serenity.Decorators.registerClass()
    export class StockFinancialReportsGrid extends Serenity.EntityGrid<StockFinancialReportsRow, any> {
        protected getColumnsKey() { return 'Default.StockFinancialReports'; }
        protected getDialogType() { return StockFinancialReportsDialog; }
        protected getIdProperty() { return StockFinancialReportsRow.idProperty; }
        protected getLocalTextPrefix() { return StockFinancialReportsRow.localTextPrefix; }
        protected getService() { return StockFinancialReportsService.baseUrl; }

        constructor(container: JQuery) {
            super(container);
        }
    }
}