
namespace HomeOfPandaEyes.StockLearning.Default {

    @Serenity.Decorators.registerClass()
    export class StocksDialog extends Serenity.EntityDialog<StocksRow, any> {
        protected getFormKey() { return StocksForm.formKey; }
        protected getIdProperty() { return StocksRow.idProperty; }
        protected getLocalTextPrefix() { return StocksRow.localTextPrefix; }
        protected getNameProperty() { return StocksRow.nameProperty; }
        protected getService() { return StocksService.baseUrl; }

        protected form = new StocksForm(this.idPrefix);

    }
}