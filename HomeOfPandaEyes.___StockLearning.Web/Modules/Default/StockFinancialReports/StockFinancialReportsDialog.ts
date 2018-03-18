
namespace HomeOfPandaEyes.StockLearning.Default {

    @Serenity.Decorators.registerClass()
    export class StockFinancialReportsDialog extends Serenity.EntityDialog<StockFinancialReportsRow, any> {
        protected getFormKey() { return StockFinancialReportsForm.formKey; }
        protected getIdProperty() { return StockFinancialReportsRow.idProperty; }
        protected getLocalTextPrefix() { return StockFinancialReportsRow.localTextPrefix; }
        protected getNameProperty() { return StockFinancialReportsRow.nameProperty; }
        protected getService() { return StockFinancialReportsService.baseUrl; }

        protected form = new StockFinancialReportsForm(this.idPrefix);

    }
}