import React from 'react';
import { Link } from 'react-router-dom';
import PropTypes from 'prop-types';
import TopbarSidebarButton from './TopbarSidebarButton';
import TopbarProfile from './TopbarProfile';
import TopbarMail from './TopbarMail';
import TopbarNotification from './TopbarNotification';
import TopbarSearch from './TopbarSearch';
import TopbarLanguage from './TopbarLanguage';
import { UserProps } from '../../../shared/prop-types/ReducerProps';
import TopbarNavLink from '../topbar_with_navigation/tobar_nav/TopbarNavLink';
import TopbarMenuLinks from './TopbarMenuLink';




const TopAdFalconMenu =  ({ user }) => user.TopMenu.items.map( x => {
  let tempClass = x.icon ;
  
     return <Link   href='#' > <i className={tempClass}></i>  {x.title}</Link>
 })


const Topbar = ({
  changeMobileSidebarVisibility, changeSidebarVisibility, user,
}) => ( 

   
  <div className="topbar">
    <div className="topbar__left">
      <TopbarSidebarButton
        changeMobileSidebarVisibility={changeMobileSidebarVisibility}
        changeSidebarVisibility={changeSidebarVisibility}
      />
      <Link className="topbar__logo" to="/default_dashboard" />
    </div>
  
    
    {<div className='customTopbarNav'>
{/*         <a className='selected' href='#'> <i class="ri-image-line"></i> Advertisers</a>
        <a href='#'> <i class="ri-folder-chart-line"></i> Reports</a>
        <a href='#'> <i class="ri-money-dollar-circle-line"></i> Deals</a>
        <a href='#'> <i class="ri-information-line"></i> Data Providers</a> */}
    
        <TopAdFalconMenu user={user}></TopAdFalconMenu>
    </div> 
    
    }
    <div className="topbar__right">
      <div className="topbar__right-search">
        <TopbarSearch />
      </div>
      <div className="topbar__right-over">
        <TopbarNotification />
        <TopbarMail new />
        <TopbarProfile user={user} />
        <TopbarLanguage />
      </div>
    </div>
  </div>
);

Topbar.propTypes = {
  changeMobileSidebarVisibility: PropTypes.func.isRequired,
  changeSidebarVisibility: PropTypes.func.isRequired,
  user: UserProps.isRequired,
};

export default Topbar;
