/* eslint-disable react/destructuring-assignment */
import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import NotificationSystem from 'rc-notification';
import Topbar from './topbar/Topbar';
import TopbarWithNavigation from './topbar_with_navigation/TopbarWithNavigation';
import Sidebar from './sidebar/Sidebar';
import SidebarMobile from './topbar_with_navigation/sidebar_mobile/SidebarMobile';
import Customizer from './customizer/Customizer';
import { BasicNotification } from '../../shared/components/Notification';
import { changeMobileSidebarVisibility, changeSidebarVisibility } from '../../redux/actions/sidebarActions';
import {
  changeThemeToDark, changeThemeToLight,
} from '../../redux/actions/themeActions';
import {
  changeDirectionToRTL, changeDirectionToLTR,
} from '../../redux/actions/rtlActions';
import { changeBorderRadius, toggleBoxShadow, toggleTopNavigation } from '../../redux/actions/customizerActions';
import {
  CustomizerProps, SidebarProps, ThemeProps, RTLProps, UserProps,
} from '../../shared/prop-types/ReducerProps';
import { changeLanguage, changeTheme, UISelectors } from '../../Store/ReducerSlices/UI';
import { ThemeTypes } from '../../Config/Themes';
//let notification = null;

// eslint-disable-next-line no-return-assign
//NotificationSystem.newInstance({ style: { top: 65 } }, n => notification = n);

const  Layout =(props) =>{




  const changeSidebarVisibility = () => {
    const { dispatch } = props;
    dispatch(changeSidebarVisibility());
  };

  const changeMobileSidebarVisibility = () => {
    const { dispatch } = props;
    dispatch(changeMobileSidebarVisibility());
  };

  const changeToDark = () => {
    const { dispatch } = props;
    dispatch(changeThemeToDark());
     dispatch(changeTheme({
         theme:  ThemeTypes.DARK 
         }));
  };

  const changeToLight = () => {
    const { dispatch } = props;
    dispatch(changeThemeToLight());
    dispatch(changeTheme({
      theme:  ThemeTypes.LIGHT 
      }));
  };

  const changeToRTL = () => {
    const { dispatch } = props;
    dispatch(changeDirectionToRTL());
  };

  const changeToLTR = () => {
    const { dispatch } = props;
    dispatch(changeDirectionToLTR());
  };

  const toggleTopNavigation = () => {
    const { dispatch } = props;
    dispatch(toggleTopNavigation());
  };

  const changeBorderRadius = () => {
    const { dispatch } = props;
    dispatch(changeBorderRadius());
  };

  const toggleBoxShadow = () => {
    const { dispatch } = props;
    dispatch(toggleBoxShadow());
  };

  const {
    customizer, sidebar, theme, rtl, user,
  } = props;
  const layoutClass = classNames({
    layout: true,
    'layout--collapse': sidebar.collapse,
    'layout--top-navigation': customizer.topNavigation,
  });
  

    return (

      
      <div className={layoutClass}>
        {customizer.topNavigation
          ? (
            <TopbarWithNavigation
              changeMobileSidebarVisibility={changeMobileSidebarVisibility}
            />
          )
          : (
           
             <Topbar
              changeMobileSidebarVisibility={changeMobileSidebarVisibility}
              changeSidebarVisibility={changeSidebarVisibility}
              user={user}
            /> 
          )
        }
        {customizer.topNavigation
          ? (
            <SidebarMobile
              sidebar={sidebar}
              changeToDark={changeToDark}
              changeToLight={changeToLight}
              changeMobileSidebarVisibility={changeMobileSidebarVisibility}
              user={user}
            />
          )
          : (
            <Sidebar
              sidebar={sidebar}
              changeToDark={changeToDark}
              changeToLight={changeToLight}
              changeMobileSidebarVisibility={changeMobileSidebarVisibility}
              user={user}
            />
          )
        }
      </div>
    );
  
}

Layout.propTypes = {
  dispatch: PropTypes.func.isRequired,
  sidebar: SidebarProps.isRequired,
  customizer: CustomizerProps.isRequired,
  theme: ThemeProps.isRequired,
  rtl: RTLProps.isRequired,
  user: UserProps.isRequired,
};

export default withRouter(connect(state => ({
  customizer: state.customizer,
  sidebar: state.sidebar,
  theme: state.theme,
  rtl: state.rtl,
  user: state.user,
}))(Layout));
