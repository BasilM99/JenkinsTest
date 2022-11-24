import { createSlice } from '@reduxjs/toolkit';
import Locales from '../../Localization/Locales';
import LocalStorage from '../../Utils/LocalStorage';
import Constants from '../../Config/Constants';
import { ThemeTypes } from '../../Config/Themes';



const ui = createSlice({
    name: 'ui',
    initialState: {
        language:   window.AdFalconUserLoggedInUserObject.CurrentLanguage,
        direction:window.AdFalconUserLoggedInUserObject.Direction  ,
        theme: ThemeTypes.DARK
    },
    reducers: {
        changeLanguage: (ui, { payload }) => {
            ui.language = payload.language;
            ui.direction = payload.direction;

            window.AdFalconUserLoggedInUserObject.CurrentLanguage=payload.language;
            window.AdFalconUserLoggedInUserObject.Direction=payload.direction;

            LocalStorage.set(Constants.keys.localStorage.uiLanguage, payload.language);


            LocalStorage.set(Constants.keys.localStorage.uiDirection, payload.direction);
        },
        changeTheme: (ui, { payload }) => {
            ui.theme = payload.theme;
            LocalStorage.set(Constants.keys.localStorage.uiTheme, payload.theme);
        }
    }
});

export const { changeLanguage, changeTheme } = ui.actions;
export default ui.reducer;



//#region :: Selectors

export const getUI = (state) => {
    return state.ui;
};
export const getUIthemeClass = (state) => {
    return state.theme;
};
export const getUIrtl = (state) => {
    return state.rtl;
};
export const getCurrentLanguage = (state) => {
    return state.ui.language;
};

export const getCurrentDirection = (state) => {
    return state.ui.direction;
};

export const getCurrentTheme = (state) => {
    return state.ui.theme;
};



export const UISelectors = {
    getUI,
    getCurrentLanguage,
    getCurrentDirection,
    getCurrentTheme,
    getUIrtl,getUIthemeClass
};

//#endregion