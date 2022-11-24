import { createSlice } from '@reduxjs/toolkit';
import { UserRoles } from '../../Config/Enums';

import {
    TopMenu_Normal,
    TopMenu_DataProvider,
    TopMenu_DSP,

    userSettingsMenu_DSP,
    userSettingsMenu_DataProvider,
    userSettingsMenu_Normal,

    menuData_DSP,
    menuData_DataProvider,
    menuData_Normal

} from '../../Components/MenuData'

const user = createSlice({
    name: 'user',
    initialState: {
        IsAdmin:window.AdFalconUserLoggedInUserObject.IsAdmin,
        IsPrimary:window.AdFalconUserLoggedInUserObject.IsPrimary,
        fullName:window.AdFalconUserLoggedInUserObject.Name,avatar:'',
        firstName: window.AdFalconUserLoggedInUserObject.FirstName,
        LastName: window.AdFalconUserLoggedInUserObject.LastName,
        impersonatedAccountName: window.AdFalconUserLoggedInUserObject.impersonatedAccountName,
        UserRole: window.AdFalconUserLoggedInUserObject.CurrentUserRole,
        SystemRoles: window.AdFalconUserLoggedInUserObject.SystemRoles,
        UserId: window.AdFalconUserLoggedInUserObject.CurrentUserId,
        AccountId:window.AdFalconUserLoggedInUserObject.CurrentLoggedAccountId,
        PermissionsList:window.AdFalconUserLoggedInUserObject.PermissionsList,
        SideMenu:
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DSPRole?menuData_DSP : 
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DataRoleProvider? menuData_DataProvider : menuData_Normal,
            TopMenu :
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DSPRole?TopMenu_DSP : 
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DataRoleProvider? TopMenu_DataProvider : TopMenu_Normal,
        UserSettingsMenu:
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DSPRole?userSettingsMenu_DSP : 
            window.AdFalconUserLoggedInUserObject.CurrentUserRole== UserRoles.DataRoleProvider? userSettingsMenu_DataProvider : userSettingsMenu_Normal,
    },
    
    reducers: {
        userInitialized: (user, action) => {
            user.firstName = action.payload.lastName;
            user.LastName = action.payload.lastName;
        },
        ChangeUserRole: (user, { payload }) => {
            user.UserRole=payload.UserRole;
           //
            user.TopMenu=payload.TopMenu;
            user.SideMenu=payload.SideMenu;
            user.UserSettingsMenu=payload.UserSettingsMenu;
            //TopMenu:
           // SideMenu:
            //UserSettingsMenu:

           // window.AdFalconUserLoggedInUserObject.CurrentLanguage=payload.language;
           // window.AdFalconUserLoggedInUserObject.Direction=payload.direction;

          //  LocalStorage.set(Constants.keys.localStorage.uiLanguage, payload.language);


           // LocalStorage.set(Constants.keys.localStorage.uiDirection, payload.direction);
        },
        userRoleInitialized: (user, action) => {
            user.UserRole = action.payload.UserRole;
            //user.LastName = action.payload.lastName;
        }
    }
});


export const { userInitialized, userRoleInitialized,ChangeUserRole} = user.actions;
export default user.reducer;

export const IsUserPermitted = (User,PermCode) => {

     return User.PermissionsList.indexOf(PermCode) > -1 ;

 
 };
 export const getForUserCurrentUserName = (state) => {
    if(state.impersonatedAccountName!='')
     return state.firstName +' ' +state.LastName+"("+ state.impersonatedAccountName+")" ;
     else
     return state.firstName +' ' +state.LastName;
 
 };
export const getCurrentUserName = (state) => {
   if(state.user.impersonatedAccountName!='')
    return state.user.firstName +' ' +state.user.LastName+"("+ state.user.impersonatedAccountName+")" ;
    else
    return state.user.firstName +' ' +state.user.LastName;

};


export const getCurrentAccountId  = (state) => {
    if(state.user.AccountId!='')
     return state.user.AccountId ;
     else
     return '' ;
 
 };

export const getCurrentUserRole= (state) => {
    return state.user.UserRole ;
};
export const getUser= (state) => {
    return state.user ;
};
export const UserSelectors = {
    getUser,
    getCurrentUserRole,
    getCurrentUserName,
    IsUserPermitted,
    getForUserCurrentUserName,
    getCurrentAccountId
};
