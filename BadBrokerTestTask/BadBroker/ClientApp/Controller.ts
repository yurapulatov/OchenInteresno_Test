import Currency from "./models/Currency";
import RateFilterModel from "./models/RateFilterModel";
import Rates from "./models/Rates";

export default class Controller {
    public static async GetResultCurrencyList() : Promise<Currency[]> {
        return this.sendQuery("https://localhost:5001/rates/currency?type=result", "GET");
    }
    public static async GetBaseCurrencyList() : Promise<Currency[]> {
        return this.sendQuery("https://localhost:5001/rates/currency?type=base", "GET");
    }
    public static async GetRates(filterModel: RateFilterModel) : Promise<Rates[]>{
        return this.sendQuery("https://localhost:5001/rates", "GET", filterModel);
    }
    
    private static async sendQuery(url: string, method: string, body?: any) : Promise<any> {
        let response = await fetch(url, {
            method: method,
            body: JSON.stringify(body)
        });
        return response.json();
    }
}
