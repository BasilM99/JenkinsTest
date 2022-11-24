import React, { useEffect } from 'react';
import { Provider as StoreProvider, useSelector,connect } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { I18nextProvider } from 'react-i18next';
import i18n from 'i18next';

import '../AdFalconApp/node_modules/bootstrap/dist/css/bootstrap.css';
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import '../../scss/app.scss';
import PropTypes from 'prop-types';
import Router from './Router';
import store from './store';
import ScrollToTop from './ScrollToTop';
import { config as i18nextConfig } from '../../translations';



import configureStore from '../../Store/ConfigureStore';



import { initReactI18next } from "react-i18next";
import HttpApi from "i18next-http-backend";

i18n.use(initReactI18next)
.use(HttpApi).init(i18nextConfig);

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

const ConnectedThemeComponent = connect(state => ({ themeName: state.theme.className }))(ThemeComponent);

const App = () => {


  useEffect(() => {
   // firebase.initializeApp(firebaseConfig);
  
  
  }, []);

  const onRedirectCallbackAuth0 = (appState) => {
    window.history.replaceState(
      {},
      document.title,
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname,
    );
  };

  const AdFalconstore = configureStore();
  return (
    <StoreProvider store={AdFalconstore}>
   
        <BrowserRouter basename="/">
          <I18nextProvider i18n={i18n}>
           
              <ScrollToTop>
                <ConnectedThemeComponent>
                  <Router />
                </ConnectedThemeComponent>
              </ScrollToTop>
       
          </I18nextProvider>
        </BrowserRouter>
    
    </StoreProvider>
  );
};

export default App;
