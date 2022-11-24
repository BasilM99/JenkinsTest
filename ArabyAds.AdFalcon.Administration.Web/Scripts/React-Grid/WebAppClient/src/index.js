//Jquery
//import $ from "jquery";
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router } from "react-router-dom";
// import createHistory from 'history/createBrowserHistory';
//Shared Code
import GlobalScope from "./GlobalScope";
import "./index.css";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";
//bootstrap
//import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-daterangepicker/daterangepicker.css";
import "bootstrap/js/tooltip.js";
import "bootstrap/js/dropdown.js";
//FontAwasome
import "font-awesome/css/font-awesome.min.css";
//Toast CSS
import "react-toastify/dist/ReactToastify.css";
//Localization
import { IntlProvider, addLocaleData } from "react-intl";
import locale_en from "react-intl/locale-data/en";
import locale_ar from "react-intl/locale-data/ar";
import strings_ar from "./translations/ar.json";
import strings_en from "./translations/en.json";
const queryString = require("query-string");
const queryValuesCMain = queryString.parse(location.search);
var langVa = "en";
if (queryValuesCMain.lang !== undefined) {
  langVa = queryValuesCMain.lang;
}


const currentLanguage = localStorage.getItem("lang") || langVa || "en";
const localStrings = {
  ar: strings_ar,
  en: strings_en
};

addLocaleData([...locale_en, ...locale_ar]);

require("bootstrap/dist/js/bootstrap.min.js");

require("./content/styles/main." + currentLanguage + ".scss");

var containerElements = document.getElementsByClassName("grid-div");

if (containerElements.length > 0) {
  for (var l = 0; l < containerElements.length; l++) {
    var stringConfig = containerElements[l].dataset.gridname;
    const gridConfig = eval("gridConfig" + stringConfig);
    const app = (
      <IntlProvider
        locale={currentLanguage}
        messages={localStrings[currentLanguage]}
      >
        <GlobalScope>
          <App gridConfigItem={gridConfig[l]} />
        </GlobalScope>
      </IntlProvider>
    );

    ReactDOM.render(app, containerElements[l]);
    //ReactDOM.render(app, containerElements);
  }
}

registerServiceWorker();
