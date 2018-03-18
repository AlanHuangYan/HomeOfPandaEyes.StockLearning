
namespace HomeOfPandaEyes.StockLearning.Default {
    export class StockFinancialReportsForm extends Serenity.PrefixedContext {
        static formKey = 'Default.StockFinancialReports';
    }

    export interface StockFinancialReportsForm {
        ReportDate: Serenity.DateEditor;
        Epsjb: Serenity.DecimalEditor;
        Epskcjb: Serenity.DecimalEditor;
        Ys: Serenity.DecimalEditor;
        Ystz: Serenity.DecimalEditor;
        Yshz: Serenity.DecimalEditor;
        Sjl: Serenity.DecimalEditor;
        Sjltz: Serenity.DecimalEditor;
        Sjlhz: Serenity.DecimalEditor;
        Bps: Serenity.DecimalEditor;
        Roepj: Serenity.DecimalEditor;
        Mgxjje: Serenity.DecimalEditor;
        Xsmll: Serenity.DecimalEditor;
        Lrfp: Serenity.StringEditor;
        Gxl: Serenity.DecimalEditor;
        NoticeDate: Serenity.DateEditor;
        UpdatedDate: Serenity.DateEditor;
    }

    [,
        ['ReportDate', () => Serenity.DateEditor],
        ['Epsjb', () => Serenity.DecimalEditor],
        ['Epskcjb', () => Serenity.DecimalEditor],
        ['Ys', () => Serenity.DecimalEditor],
        ['Ystz', () => Serenity.DecimalEditor],
        ['Yshz', () => Serenity.DecimalEditor],
        ['Sjl', () => Serenity.DecimalEditor],
        ['Sjltz', () => Serenity.DecimalEditor],
        ['Sjlhz', () => Serenity.DecimalEditor],
        ['Bps', () => Serenity.DecimalEditor],
        ['Roepj', () => Serenity.DecimalEditor],
        ['Mgxjje', () => Serenity.DecimalEditor],
        ['Xsmll', () => Serenity.DecimalEditor],
        ['Lrfp', () => Serenity.StringEditor],
        ['Gxl', () => Serenity.DecimalEditor],
        ['NoticeDate', () => Serenity.DateEditor],
        ['UpdatedDate', () => Serenity.DateEditor]
    ].forEach(x => Object.defineProperty(StockFinancialReportsForm.prototype, <string>x[0], {
        get: function () {
            return this.w(x[0], (x[1] as any)());
        },
        enumerable: true,
        configurable: true
    }));
}