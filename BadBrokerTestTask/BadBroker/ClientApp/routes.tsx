import * as React from "react";
import { Switch, Route } from "react-router-dom";
import MainPage from "./pages/MainPage/MainPage";

export const Routes = () => {
    return (
        <Switch>
            <Route exact path="/" component={MainPage} />
        </Switch>
    );
}