
namespace HomeOfPandaEyes.StockLearning.Default {
    export interface StocksRow {
        StockId?: string;
        StockName?: string;
    }

    export namespace StocksRow {
        export const idProperty = 'StockId';
        export const nameProperty = 'StockId';
        export const localTextPrefix = 'Default.Stocks';

        export namespace Fields {
            export declare const StockId;
            export declare const StockName;
        }

        [
            'StockId',
            'StockName'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}