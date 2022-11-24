import {
  CHANGE_THEME_TO_DARK,
  CHANGE_THEME_TO_LIGHT,
} from '../actions/themeActions';

const initialState = {
  className:  window.AdFalconUserLoggedInUserObject.themeClass  ,
};

const themeReducer = (state = initialState, action) => {
  switch (action.type) {
    case CHANGE_THEME_TO_DARK:
      window.AdFalconUserLoggedInUserObject.themeClass='theme-dark';
      return { className: 'theme-dark' };
    case CHANGE_THEME_TO_LIGHT:
      window.AdFalconUserLoggedInUserObject.themeClass='theme-light';
      return { className: 'theme-light' };
    default:
      return state;
  }
};

export default themeReducer;
