import { combineReducers } from '@reduxjs/toolkit';
import userReducer from './ReducerSlices/User';
import uiReducer from './ReducerSlices/UI';
import entitiesReducer from './ReducerSlices/Entities';
import themeReducer from '../redux/reducers/themeReducer';
import rtlReducer from '../redux/reducers/rtlReducer';
import sidebarReducer from '../redux/reducers/sidebarReducer';
import cryptoTableReducer from '../redux/reducers/cryptoTableReducer';
import newOrderTableReducer from '../redux/reducers/newOrderTableReducer';
import customizerReducer from '../redux/reducers/customizerReducer';
import todoReducer from '../redux/reducers/todoReducer';
import authReducer from '../redux/reducers/authReducer';
import appConfigReducer from '../redux/reducers/appConfigReducer';
import { reducer as reduxFormReducer } from 'redux-form';
export default combineReducers({
    user: userReducer,
    ui: uiReducer,
    entities: entitiesReducer,
    form: reduxFormReducer, // mounted under "form",
  theme: themeReducer,
  rtl: rtlReducer,
  appConfig: appConfigReducer,
  cryptoTable: cryptoTableReducer,
  customizer: customizerReducer,
  newOrder: newOrderTableReducer,
  sidebar: sidebarReducer,
  //user: authReducer,
  //covid: covidReducer,
  todo: todoReducer,


});