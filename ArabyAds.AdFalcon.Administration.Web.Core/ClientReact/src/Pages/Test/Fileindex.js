import React from 'react';
import ReactDOM from 'react-dom';
import { Provider as StoreProvider, useSelector } from 'react-redux';
import ScrollToTop from '../../containers/App/ScrollToTop';
import '../../containers/AdFalconApp/node_modules/bootstrap/dist/css/bootstrap.css';
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import '../../scss/app.scss';



import LocaleProvider from '../../Localization/LocaleProvider';
import ErrorBoundary from '../../Components/Errors/ErrorBoundary';
import configureStore from '../../Store/ConfigureStore';
import { UISelectors } from '../../Store/ReducerSlices/UI';
import Themes from '../../Config/Themes';
import App from './App.jsx';
import { ThemeTypes } from '../../Config/Themes';
import PropTypes from 'prop-types';
import { connect, Provider } from 'react-redux';
import i18next from "i18next";
import { initReactI18next } from "react-i18next";
import HttpApi from "i18next-http-backend";
import { useTranslation } from "react-i18next";
i18next
  .use(initReactI18next)
  .use(HttpApi)
  .init({
    lng: "en",  load: 'currentOnly',
    ns:["namespace2","Campaign"],
    partialBundledLanguages :true,
    backend:{ loadPath: 'https://localhost:44328/en/Common/Resources?langName={{lng}}&setName={{ns}}',


 
  

    allowMultiLoading: false},
    react: {
      wait: true,
      useSuspense: false,
   },
  
defaultNS:'namespace2',
resources: {
  en: {
    namespace1: {
      key: 'hello from namespace 1'
    },
    namespace2: {
      key: 'hello from namespace 2'
    }
  },
  ar: {
    namespace1: {
      key: 'hallo von namespace 1'
    },
    namespace2: {
      key: 'hallo von namespace 2'
    }  
  }
}
,
   
    supportedLngs: ["en", "ar"],
    // Allows "en-US" and "en-UK" to be implcitly supported when "en"
    // is supported
    nonExplicitSupportedLngs: true,
    fallbackLng: "en",
    interpolation: {
      escapeValue: false,
    },
    debug:true,
  }, (error, t) => {
   // debugger;
    if(error)
        console.error(error);
});
const ThemeComponent = ({ children, themeName }) => {
  const theme = createMuiTheme({
    palette: {
      type: themeName === 'theme-dark' ? 'dark' : 'light',
    },
  });

  return (
    <ThemeProvider theme={theme}>
      {children}
    </ThemeProvider>
  );
};

ThemeComponent.propTypes = {
  children: PropTypes.shape().isRequired,
  themeName: PropTypes.string.isRequired,
};

const ConnectedThemeComponent = connect(state => ({ themeName: state.theme.className  }))(ThemeComponent);
//import '../../Assets/KendoUI/variables.scss';
// import '@progress/kendo-theme-default/dist/all.css';
/*
import 'bootstrap/dist/css/bootstrap.min.css';*/
//import '../../Assets/KendoUI/adfalcon.css';

/* 
//STEP 1:
//create components using React.lazy
const LightThemeSelector =import('../../Components/LightThemeSelector');
const DarkThemeSelector =   import('../../Components/DarkThemeSelector');

//STEP 2:
//create a parent component that will load the components conditionally using React.Suspense
const ThemeSelector = ({ children }) => {
  const state = useSelector(state => state);

    const uiTheme = UISelectors.getCurrentTheme(state);

  const CHOSEN_THEME = uiTheme;
  return (
    <>
      <React.Suspense fallback={<></>}>
        {(CHOSEN_THEME === ThemeTypes.light) && <LightThemeSelector />}
        {(CHOSEN_THEME === ThemeTypes.dark) && <DarkThemeSelector />}
      </React.Suspense>
      {children}
    </>
  )
} */

//#region :: Main 
const Main = ({ }) => {


  // load additional namespaces after initialization
i18next.loadNamespaces('Global', (err, t) => { /* ... */ });
    const state = useSelector(state => state);
    const uiLang = UISelectors.getCurrentLanguage(state);
    const uiDir = UISelectors.getCurrentDirection(state);
    const uiTheme = UISelectors.getCurrentTheme(state);
    const {t, i18n } = useTranslation();
    return <>
        <LocaleProvider locale={uiLang} >
       
           
                 
                  <ConnectedThemeComponent>
                
                
                <span> {t("namespace2:key")}   {t("Global:CreationDate")}</span>   
    </ConnectedThemeComponent>
           
   
                  

                  
          
       
        </LocaleProvider>
    </>;
};
//#endregion

//#region  Render React App
const rootContainer = 'APP-Root';
const store = configureStore();

ReactDOM.render(<>
    <StoreProvider store={store}>
        <Main />
    </StoreProvider >
</>, document.getElementById(rootContainer));

//#endregion