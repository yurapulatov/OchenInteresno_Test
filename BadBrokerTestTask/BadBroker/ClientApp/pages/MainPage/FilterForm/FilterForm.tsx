import React, {useEffect, useState} from "react";
import Currency from "../../../models/Currency";
import Controller from "../../../Controller";
import RateFilterModel from "../../../models/RateFilterModel";
import "./FilterForm.less"
import moment from "moment";

interface FilterFormProps {
    onClickFind: (rateFilterModel: RateFilterModel) => void
}

class CheckBoxCurrencyModel {
    currency: Currency;
    isChecked: boolean;
}

export default function FilterForm(props: FilterFormProps) {
    /*Initial Data*/
    const [baseCurrenciesList, setBaseCurrenciesList] = useState<Currency[]>([]);
    const [resultCurrenciesList, setResultCurrenciesList] = useState<CheckBoxCurrencyModel[]>([]);
    const [dataIsLoading, setIsLoading] = useState<boolean>(false);
    
    /*Result Data*/
    const [selectedBaseCurrencyCode, setSelectedBaseCurrencyCode] = useState<string>("");
    const [selectedStartDate, setStartDate] = useState<string>(null);
    const [selectedEndDate, setEndDate] = useState<string>(null);
    const [valueMoney, setValueMoney] = useState<number>(0);
    
    useEffect( () => {
        if (!dataIsLoading && baseCurrenciesList.length == 0 && resultCurrenciesList.length == 0) {
            setIsLoading(true);
            Controller.GetBaseCurrencyList().then(
                (data: Currency[]) => { 
                    setBaseCurrenciesList(data);
                    setSelectedBaseCurrencyCode(data.find(x => x).code)
                }
            );
            Controller.GetResultCurrencyList().then(
                (data: Currency[]) => {
                    setResultCurrenciesList(data.map(x => {
                        let model = new CheckBoxCurrencyModel();
                        model.currency = x;
                        model.isChecked = false;
                        return model;
                    }));
                }
            );
        }
    });
    
    function onFind() {
        let filterForm = new RateFilterModel();
        filterForm.baseCurrency = baseCurrenciesList.find(x => x.code == selectedBaseCurrencyCode);
        filterForm.dateFrom = selectedStartDate;
        filterForm.dateTo = selectedEndDate;
        filterForm.resultCurrencyList = resultCurrenciesList.filter(x => x.isChecked).map(x => x.currency);
        filterForm.inputValueMoney = valueMoney;
        props.onClickFind(filterForm);
    }
    
    function onChangeSelectedResultCurrencies(id: number, isChecked: boolean) {
        let index = resultCurrenciesList.findIndex(x => x.currency.id == id);
        let newArray =  [...resultCurrenciesList];
        newArray[index] = {...newArray[index], isChecked: isChecked};
        setResultCurrenciesList(newArray);
    }
    
    let disabledFind = resultCurrenciesList.filter(x => x.isChecked).length == 0 || selectedEndDate == null || selectedStartDate == null || selectedBaseCurrencyCode == "";
    
    return (
        <div className={"filter_form"}>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Input your money value</div>
                <input type={"number"} 
                       value={valueMoney} 
                       onChange={(e) => setValueMoney(parseInt(e.target.value))}
                       min={0}
                />
            </div>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Select base currency</div>
                <select value={selectedBaseCurrencyCode} onChange={(event) => setSelectedBaseCurrencyCode(event.target.value)}>
                    {baseCurrenciesList.map( (x, index) => {
                        return <option key={index} value={x.code}>{`${x.code} (${x.name})`}</option>
                    })}
                </select>
            </div>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Checking result currency</div>
                {resultCurrenciesList.length > 0 && resultCurrenciesList.map( (x, index) => {
                    return <div>
                        <input name={'result_currency'}
                               key={index}
                               type={"checkbox"} 
                               onChange={(event => { onChangeSelectedResultCurrencies(x.currency.id, !x.isChecked)})}
                               value={x.isChecked.toString()}
                        />
                        <label>{`${x.currency.code} (${x.currency.name})`}</label>
                    </div>
                })}
            </div>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Choose filter date range</div>
                <div className={"filter_form__item-datepicker"}>
                    <input onChange={(event) => setStartDate(event.target.value)}
                           type={"date"}
                           name={"start_date"}
                           max={moment(new Date()).format("YYYY-MM-DD")}
                    />
                    <input onChange={(event) => setEndDate(event.target.value)}
                           type={"date"}
                           name={"end_date"}
                           max={moment(new Date()).format("YYYY-MM-DD")}
                    />
                </div>

            </div>
            <div className={"filter_form__button_submit"}>
                <button disabled={disabledFind} onClick={() => onFind()}>Find</button>
            </div>
        </div>

        
    )
}