import React from "react";
import Rates from "../../../models/Rates";
import moment from "moment";

export interface TableResultFormProps {
    rates: Rates[],
    startDate: string,
    endDate: string
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
    
    function renderTableValue() {
        var dateArray = getArrayDates();
        return dateArray.map(date => {
            return <tr>
                <td>
                    {date}
                </td>
                {
                    props.rates.map(rate => {
                        var item = rate.ratesInfoList.find(x => moment(x.date).format('YYYY-MM-DD') == date);
                        return <td>{item.value}</td>
                    })
                }
            </tr>

        })
    }
    
    return <div>
        <table>
            <thead>
            <tr>
                <td>Date</td>
                {props.rates.map(x => {
                    return <td>{`${x.baseCurrency.code}/${x.resultCurrency.code}`}</td>
                })}
            </tr>
            </thead>
            <tbody>
            {renderTableValue()}
            </tbody>
        </table>
    </div>

}