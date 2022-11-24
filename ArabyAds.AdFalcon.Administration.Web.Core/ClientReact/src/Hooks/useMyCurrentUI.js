import { useSelector, useDispatch } from 'react-redux';

import { changeLanguage, changeTheme, UISelectors } from '../Store/ReducerSlices/UI';
import { userInitialized, userRoleInitialized, ChangeUserRole, UserSelectors, IsUserPermitted } from '../Store/ReducerSlices/User';
const useMyCurentUI = () => {

  const stateStore = useSelector(state => state);

const UI = UISelectors.getUI(stateStore);
const getCurrentTheme = UISelectors.getUIthemeClass(stateStore);
const getDirection= () => {
        
        
  return UI.direction;


};
const getLanguage= () => {
        
        
  return UI.language;


};
const getTheme= () => {
        
        
  return getCurrentTheme;


};
  
    return { getDirection, getLanguage, getTheme ,UI }
  }

  export default useMyCurentUI;