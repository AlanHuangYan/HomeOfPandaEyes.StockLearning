
namespace HomeOfPandaEyes.StockLearning.Default {
    export class StocksForm extends Serenity.PrefixedContext {
        static formKey = 'Default.Stocks';
    }

    export interface StocksForm {
        StockName: Serenity.StringEditor;
    }

    [,
        ['StockName', () => Serenity.StringEditor]
    ].forEach(x => Object.defineProperty(StocksForm.prototype, <string>x[0], {
        get: function () {
            return this.w(x[0], (x[1] as any)());
        },
        enumerable: true,
        configurable: true
    }));
}