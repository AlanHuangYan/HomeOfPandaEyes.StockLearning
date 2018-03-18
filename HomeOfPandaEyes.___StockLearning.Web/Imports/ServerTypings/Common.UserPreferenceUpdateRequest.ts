namespace HomeOfPandaEyes.StockLearning.Common {
    export interface UserPreferenceUpdateRequest extends Serenity.ServiceRequest {
        PreferenceType?: string;
        Name?: string;
        Value?: string;
    }
}

