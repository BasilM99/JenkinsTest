import { useSelector, useDispatch } from 'react-redux';

import { changeLanguage, changeTheme, UISelectors } from '../Store/ReducerSlices/UI';
import { userInitialized, userRoleInitialized, ChangeUserRole, UserSelectors, IsUserPermitted } from '../Store/ReducerSlices/User';
const useCurentUser = () => {

    const stateStore = useSelector(state => state);
	//const UI = UISelectors.getUI(stateStore);

    const UserGlobal = UserSelectors.getUser(stateStore);
    

     const IsUserPermitted = (PermCode) => {
        
        
         return UserGlobal.PermissionsList.includes(PermCode)  ;
    
     
     };
     const IsUserInSystemRole = (SystemRole) => {
        
        
      return UserGlobal.SystemRoles.includes(SystemRole) ;
 
  
  };
      const IsUserHasRole = (UserRole) => {
       // const stateStoreUser = useSelector(state => state.user);
        
         return UserGlobal.UserRole===UserRole ;
    
     
     };
     const IsUserPrimary = () => {
        // const stateStoreUser = useSelector(state => state.user);
         
          return UserGlobal.IsPrimary;
     
      
      };
      const IsAdmin = (UserRole) => {
        // const stateStoreUser = useSelector(state => state.user);
         
          return UserGlobal.IsAdmin;
     
      
      };
    
  
    return { IsUserPermitted, IsUserHasRole, IsUserPrimary,IsAdmin,IsUserInSystemRole,UserGlobal }
  }

  export default useCurentUser;