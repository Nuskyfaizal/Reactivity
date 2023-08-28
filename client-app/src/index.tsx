import React from "react";
import ReactDOM from 'react-dom/client';
import { RouterProvider } from "react-router-dom";
import "react-toastify/dist/ReactToastify.min.css";
import 'react-calendar/dist/Calendar.css';
import "react-datepicker/dist/react-datepicker.css";
import "semantic-ui-css/semantic.min.css";
import "./app/layout/styles.css";
import { store, StoreContext } from "./app/stores/store";
import reportWebVitals from "./reportWebVitals";
import { router } from "./app/router/Routes";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StoreContext.Provider value={store}>
    <RouterProvider router={router} />
  </StoreContext.Provider>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
//serviceWorker.unregister();
reportWebVitals();