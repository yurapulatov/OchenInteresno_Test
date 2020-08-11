import React from "react";
import Rates from "../../../models/Rates";
import moment from "moment";
import "./TableResultForm.less"

export interface TableResultFormProps {
    rates: Rates[],
    startDate: string,
    endDate: string,
    inputMoney: number
}

export default function TableResultForm (props: TableResultFormProps) {
    function getArrayDates() {
        var dateArray = [];
        var currentDate = moment(props.startDate);
        var stopDate = moment(props.endDate);
        while (currentDate <= stopDate) {
            dateArray.push( moment(currentDate).format('YYYY-MM-DD') )
            currentDate = moment(currentDate).add(1, 'days');
        }
        return dateArray;
    }

    function renderHeaderTable() {
        return <tr>
            <td className={"table_result_form__cell"}>
                <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>Date</div>
            </td>
            {props.rates.map(x => {
                return <td className={"table_result_form__cell"}>
                    <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>
                        {`${x.resultCurrency.code}/${x.baseCurrency.code}`}
                    </div>
                </td>
            })}
        </tr>
    }
    
    function renderTotalValue() {
        return ( <tfoot>
            <tr>
                <td className={"table_result_form__cell"}>
                    <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>
                        Your input
                    </div>
                </td>
                {
                    props.rates.map(rate => {
                        return <td className={"table_result_form__cell"}>
                            <div className={"table_result_form__cell__value"}>
                                {props.inputMoney}
                            </div>
                        </td>
                    })
                }
            </tr>
            <tr>
                <td className={"table_result_form__cell"}>
                    <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>
                        Your incoming
                    </div>
                </td>
                {
                    props.rates.map(rate => {
                        return <td className={"table_result_form__cell"}>
                            <div className={"table_result_form__cell__value"}>
                                {rate.maxMoneyValue - props.inputMoney}
                            </div>
                        </td>

                    })
                }
            </tr>
            <tr>
                <td className={"table_result_form__cell"}>
                    <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>
                        Your output
                    </div>
                </td>
                {
                    props.rates.map(rate => {
                        return <td className={"table_result_form__cell"}>
                            <div className={"table_result_form__cell__value"}>
                                {rate.maxMoneyValue}
                            </div>
                        </td>

                    })
                }
            </tr>
            </tfoot>
        )

    }
    
    
    function renderTableValue() {
        var dateArray = getArrayDates();
        return dateArray.map(date => {
            return <tr>
                <td className={"table_result_form__cell"}>
                    <div className={"table_result_form__cell__value table_result_form__cell__value-header"}>
                        {date}
                    </div>
                </td>
                {
                    props.rates.map(rate => {
                        var item = rate.ratesInfoList.find(x => moment(x.date).format('YYYY-MM-DD') == date);
                        var bestStart = moment(rate.bestStartDate).format('YYYY-MM-DD') == date;
                        var bestEnd = moment(rate.bestEndingDate).format('YYYY-MM-DD') == date;
                        var bestInterval = moment(rate.bestEndingDate) > moment(date) && moment(rate.bestStartDate) < moment(date);
                        let className = "";
                        if (bestStart) {
                            className = "table_result_form__cell-best_start";
                        }
                        if (bestInterval) {
                            className = "table_result_form__cell-best-interval";
                        }
                        if (bestEnd) {
                            className = "table_result_form__cell-best-end";
                        }
                        
                        if (item != null) {

                            return <td className={"table_result_form__cell " + className}>
                                <div className={"table_result_form__cell__value"}>
                                    {item.value}
                                </div>
                            </td>
                        }
                        else {
                            return <td className={"table_result_form__cell " + className}>
                                <div className={"table_result_form__cell__value table_result_form__cell__value-holiday"}>
                                    Weekend
                                </div>
                            </td>
                        }
                    })
                }
            </tr>

        })
    }
    if (props.rates.length > 0) {
        return <div className={"table_result_form"}>
            <table>
                <thead>
                {renderHeaderTable()}
                </thead>
                <tbody>
                {renderTableValue()}
                </tbody>
                {renderTotalValue()}
            </table>
        </div>
    }
    else {
        return <div className={"table_result_form table_result_form__nothing"}>
                Nothing to show. Fill your filter and search.
        </div>
    }

}