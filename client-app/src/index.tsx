import React from "react";
import ReactDOM from "react-dom";
import { Router } from "react-router-dom";
import { createBrowserHistory } from 'history';
import "react-toastify/dist/ReactToastify.min.css";
import 'react-calendar/dist/Calendar.css';
import "react-datepicker/dist/react-datepicker.css";
import "semantic-ui-css/semantic.min.css";
import "./app/layout/styles.css";
import App from "./app/layout/App";
import { store, StoreContext } from "./app/stores/store";
import reportWebVitals from "./reportWebVitals";

export const history = createBrowserHistory();

ReactDOM.render(
  <StoreContext.Provider value={store}>
    <Router history={history}>
    <App />
  </Router>
  </StoreContext.Provider>, 
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
//serviceWorker.unregister();
reportWebVitals();