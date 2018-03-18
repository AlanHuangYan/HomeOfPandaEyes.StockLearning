
namespace HomeOfPandaEyes.StockLearning.Default {
    export interface StockFinancialReportsRow {
        StockId?: string;
        ReportDate?: string;
        Epsjb?: number;
        Epskcjb?: number;
        Ys?: number;
        Ystz?: number;
        Yshz?: number;
        Sjl?: number;
        Sjltz?: number;
        Sjlhz?: number;
        Bps?: number;
        Roepj?: number;
        Mgxjje?: number;
        Xsmll?: number;
        Lrfp?: string;
        Gxl?: number;
        NoticeDate?: string;
        UpdatedDate?: string;
        StockStockName?: string;
    }

    export namespace StockFinancialReportsRow {
        export const idProperty = 'StockId';
        export const nameProperty = 'StockId';
        export const localTextPrefix = 'Default.StockFinancialReports';

        export namespace Fields {
            export declare const StockId;
            export declare const ReportDate;
            export declare const Epsjb;
            export declare const Epskcjb;
            export declare const Ys;
            export declare const Ystz;
            export declare const Yshz;
            export declare const Sjl;
            export declare const Sjltz;
            export declare const Sjlhz;
            export declare const Bps;
            export declare const Roepj;
            export declare const Mgxjje;
            export declare const Xsmll;
            export declare const Lrfp;
            export declare const Gxl;
            export declare const NoticeDate;
            export declare const UpdatedDate;
            export declare const StockStockName;
        }

        [
            'StockId',
            'ReportDate',
            'Epsjb',
            'Epskcjb',
            'Ys',
            'Ystz',
            'Yshz',
            'Sjl',
            'Sjltz',
            'Sjlhz',
            'Bps',
            'Roepj',
            'Mgxjje',
            'Xsmll',
            'Lrfp',
            'Gxl',
            'NoticeDate',
            'UpdatedDate',
            'StockStockName'
        ].forEach(x => (<any>Fields)[x] = x);
    }
}