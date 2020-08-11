import Currency from "./Currency";

export default class RateFilterModel {
    public dateFrom: string;
    public dateTo: string;
    public baseCurrency: Currency;
    public resultCurrencyList: Currency[];
    public inputValueMoney: number;
}