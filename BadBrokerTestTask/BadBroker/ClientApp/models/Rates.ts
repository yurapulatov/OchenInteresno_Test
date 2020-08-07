import Currency from "./Currency";
import RatesInfo from "./RatesInfo";

export default class Rates {
    public resultCurrency: Currency;
    public baseCurrency: Currency;
    public ratesInfoList: RatesInfo[];
    public bestStartDate: string;
    public bestEndingDate: string;
}