import * as React from "react";
import { Router } from "react-router-dom";
import { createBrowserHistory } from "history";
import {Routes} from "./routes";

export const history = createBrowserHistory();

export const App = () => {
    const [name] = React.useState("App");

    return (
        <div className={name}>
            <Router history={history}>
                <Routes/>
            </Router>
        </div>
    );
}