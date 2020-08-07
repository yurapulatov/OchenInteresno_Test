import React, {useEffect, useState} from "react";
import Currency from "../../../models/Currency";
import Controller from "../../../Controller";
import RateFilterModel from "../../../models/RateFilterModel";
import "./FilterForm.less"

interface FilterFormProps {
    onClickFind: (rateFilterModel: RateFilterModel) => void
}

export default function FilterForm(props: FilterFormProps) {
    /*Initial Data*/
    const [baseCurrenciesList, setBaseCurrenciesList] = useState<Currency[]>([]);
    const [resultCurrenciesList, setResultCurrenciesList] = useState<Currency[]>([]);
    const [dataIsLoadingBaseCurrencyList, setIsLoadingBaseCurrencyList] = useState<boolean>(false);
    const [dataIsLoadingResultCurrencyList, setIsLoadingResultCurrencyList] = useState<boolean>(false);
    
    /*Result Data*/
    const [selectedBaseCurrency, setSelectedBaseCurrency] = useState<Currency>(null);
    const [selectedResultCurrenciesList, setSelectedResultCurrenciesList] = useState<Currency[]>([]);
    const [selectedStartDate, setStartDate] = useState<string>(null);
    const [selectedEndDate, setEndDate] = useState<string>(null);
    
    useEffect( () => {
        if (!dataIsLoadingBaseCurrencyList) {
            Controller.GetBaseCurrencyList().then(
                (data: Currency[]) => { 
                    setBaseCurrenciesList(data);
                    setIsLoadingBaseCurrencyList(true);
                }
            );
        }
        if (!dataIsLoadingResultCurrencyList) {
            Controller.GetResultCurrencyList().then(
                (data: Currency[]) => { 
                    setResultCurrenciesList(data);
                    setIsLoadingResultCurrencyList(true);
                }
            );
        }
    });
    
    function onFind() {
        let filterForm = new RateFilterModel();
        filterForm.baseCurrency = selectedBaseCurrency;
        filterForm.dateFrom = selectedStartDate;
        filterForm.dateTo = selectedEndDate;
        filterForm.resultCurrencyList = selectedResultCurrenciesList;
        props.onClickFind(filterForm);
    }
    
    function onSelectedBaseCurrency(value: string) {
        setSelectedBaseCurrency(baseCurrenciesList.find(x => x.code == value));
    }
    
    let disabledFind = selectedResultCurrenciesList.length == 0 || selectedEndDate == null || selectedStartDate == null || selectedBaseCurrency == null;
    
    return (
        <div className={"filter_form"}>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Select base currency</div>
                <select onChange={(event) => onSelectedBaseCurrency(event.target.value)}>
                    {baseCurrenciesList.map( x => {
                        return <option value={x.code}>{`${x.code} (${x.name})`}</option>
                    })}
                </select>
            </div>
            <div className={"filter_from__item"}>
                <div className={"filter_form__title"}>Checking result currency</div>
                {resultCurrenciesList.length > 0 && resultCurrenciesList.map( x => {
                    return <div>
                        <input name={'result_currency'} type={"checkbox"}/>
                        <label>{`${x.code} (${x.name})`}</label>
                    </div>
                })}
            </div>
            <div className={"filter_form__item"}>
                <div className={"filter_form__title"}>Choose filter date range</div>
                <input onChange={(event) => setStartDate(event.target.value)} 
                       type={"date"}
                       name={"start_date"}/>
                <input onChange={(event) => setEndDate(event.target.value)} 
                       type={"date"} 
                       name={"end_date"}/>
            </div>
            <div className={"filter_form__button_submit"}>
                <button disabled={disabledFind} onClick={() => onFind()}>Find</button>
            </div>
        </div>

        
    )
}