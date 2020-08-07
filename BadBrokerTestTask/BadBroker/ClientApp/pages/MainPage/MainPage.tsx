import React, {useState} from "react";
import "./MainPage.less"
import FilterForm from "./FilterForm/FilterForm";
import RateFilterModel from "../../models/RateFilterModel";
import Controller from "../../Controller";
import Rates from "../../models/Rates";
import TableResultForm from "./TableResultForm/TableResultForm";

export default function MainPage() {
    const [filterModel, setFilterModel] = useState<RateFilterModel>(new RateFilterModel());
    const [rates, setRates] = useState<Rates[]>([]);
    
    function onClickFind(filterModelArg: RateFilterModel) {
        Controller.GetRates(filterModelArg).then(
            (data: Rates[]) => {
                setFilterModel(filterModelArg);
                setRates(data);
            }
        )
    }
    
    return <div className={"main_page"}>
        <FilterForm onClickFind={(filterModel: RateFilterModel) => onClickFind(filterModel)}/>
        {rates.length > 0 && <TableResultForm rates={rates} startDate={filterModel.dateFrom} endDate={filterModel.dateTo}/>}
    </div>
}